using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VyacheslavsMegaWebApp.Data;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CapesEditorController : Controller
    {
        private readonly AppDbContext _db;
        private readonly SkinStorageService _skinStorage;
        private readonly IWebHostEnvironment _env;

        public CapesEditorController(AppDbContext db, SkinStorageService skinStorage, IWebHostEnvironment env)
        {
            _db = db;
            _skinStorage = skinStorage;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _db.CustomCapes.AnyAsync(c => c.Slug == "vec"))
            {
                DataSeeder.SeedDefaultCapes(_db, _env);
            }

            var capes = await _db.CustomCapes
                .Include(c => c.Codes)
                .Include(c => c.Ownerships)
                .OrderBy(c => c.Slug == "vec" ? 0 : 1)
                .ThenByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(capes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string? slug, string? description, bool isPublic, IFormFile? capeImage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Укажите название плаща.";
                return View();
            }

            if (capeImage == null || capeImage.Length == 0)
            {
                TempData["Error"] = "Загрузите файл текстуры плаща (.png).";
                return View();
            }

            if (!capeImage.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Файл плаща должен быть в формате .png.";
                return View();
            }

            string cleanSlug;
            if (string.IsNullOrWhiteSpace(slug))
            {
                cleanSlug = "cape_" + Guid.NewGuid().ToString("N")[..8];
            }
            else
            {
                cleanSlug = new string(slug.Trim().ToLowerInvariant().Where(ch => char.IsLetterOrDigit(ch) || ch == '_' || ch == '-').ToArray());
                if (string.IsNullOrEmpty(cleanSlug))
                {
                    cleanSlug = "cape_" + Guid.NewGuid().ToString("N")[..8];
                }
            }

            if (await _db.CustomCapes.AnyAsync(c => c.Slug == cleanSlug))
            {
                TempData["Error"] = $"Плащ с идентификатором «{cleanSlug}» уже существует.";
                return View();
            }

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await capeImage.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            var hash = _skinStorage.SaveTextureBytes(bytes);

            var cape = new CustomCape
            {
                Name = name.Trim(),
                Slug = cleanSlug,
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                TextureHash = hash,
                IsPublic = isPublic,
                CreatedAt = DateTime.UtcNow
            };

            _db.CustomCapes.Add(cape);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Плащ «{cape.Name}» успешно добавлен.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cape = await _db.CustomCapes.FindAsync(id);
            if (cape == null)
            {
                TempData["Error"] = "Плащ не найден.";
                return RedirectToAction(nameof(Index));
            }

            return View(cape);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string? slug, string? description, bool isPublic, IFormFile? newImage)
        {
            var cape = await _db.CustomCapes.FindAsync(id);
            if (cape == null)
            {
                TempData["Error"] = "Плащ не найден.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Название плаща не может быть пустым.";
                return View(cape);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                var cleanSlug = new string(slug.Trim().ToLowerInvariant().Where(ch => char.IsLetterOrDigit(ch) || ch == '_' || ch == '-').ToArray());
                if (!string.IsNullOrEmpty(cleanSlug) && cleanSlug != cape.Slug)
                {
                    if (await _db.CustomCapes.AnyAsync(c => c.Slug == cleanSlug && c.Id != id))
                    {
                        TempData["Error"] = $"Идентификатор «{cleanSlug}» уже используется другим плащом.";
                        return View(cape);
                    }
                    cape.Slug = cleanSlug;
                }
            }

            cape.Name = name.Trim();
            cape.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            cape.IsPublic = isPublic;

            if (newImage != null && newImage.Length > 0)
            {
                if (!newImage.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Error"] = "Файл должен быть в формате .png.";
                    return View(cape);
                }

                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    await newImage.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

                var hash = _skinStorage.SaveTextureBytes(bytes);
                cape.TextureHash = hash;
            }

            await _db.SaveChangesAsync();

            TempData["Success"] = $"Плащ «{cape.Name}» успешно обновлен.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Codes(int id)
        {
            var cape = await _db.CustomCapes
                .Include(c => c.Codes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cape == null)
            {
                TempData["Error"] = "Плащ не найден.";
                return RedirectToAction(nameof(Index));
            }

            var userIds = cape.Codes.Where(c => c.IsUsed && !string.IsNullOrEmpty(c.RedeemedByUserId))
                .Select(c => c.RedeemedByUserId!)
                .Distinct()
                .ToList();

            var usersMap = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName);

            ViewBag.UsersMap = usersMap;

            return View(cape);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateCodes(int capeId, int count, string? prefix)
        {
            var cape = await _db.CustomCapes.FindAsync(capeId);
            if (cape == null)
            {
                TempData["Error"] = "Плащ не найден.";
                return RedirectToAction(nameof(Index));
            }

            count = Math.Clamp(count, 1, 500);
            var cleanPrefix = string.IsNullOrWhiteSpace(prefix) ? "VEC" : prefix.Trim().ToUpperInvariant();

            const string chars = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            var codes = new List<CapeRedeemCode>();
            var existingCodes = (await _db.CapeRedeemCodes.Select(c => c.Code).ToListAsync()).ToHashSet();

            for (int i = 0; i < count; i++)
            {
                string code;
                int attempts = 0;
                do
                {
                    code = $"{cleanPrefix}-{RandomString(chars, 4)}-{RandomString(chars, 4)}-{RandomString(chars, 4)}";
                    attempts++;
                } while (existingCodes.Contains(code) && attempts < 20);

                existingCodes.Add(code);
                codes.Add(new CapeRedeemCode
                {
                    CapeId = capeId,
                    Code = code,
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _db.CapeRedeemCodes.AddRangeAsync(codes);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Сгенерировано промокодов: {codes.Count}.";
            return RedirectToAction(nameof(Codes), new { id = capeId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCode(int id, int capeId)
        {
            var code = await _db.CapeRedeemCodes.FirstOrDefaultAsync(c => c.Id == id && c.CapeId == capeId);
            if (code != null)
            {
                _db.CapeRedeemCodes.Remove(code);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Промокод удален.";
            }

            return RedirectToAction(nameof(Codes), new { id = capeId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCape(int id)
        {
            var cape = await _db.CustomCapes.FindAsync(id);
            if (cape == null)
            {
                TempData["Error"] = "Плащ не найден.";
                return RedirectToAction(nameof(Index));
            }

            if (cape.Slug == "vec")
            {
                TempData["Error"] = "Базовый плащ VEC не может быть удален.";
                return RedirectToAction(nameof(Index));
            }

            _db.CustomCapes.Remove(cape);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Плащ «{cape.Name}» удален.";
            return RedirectToAction(nameof(Index));
        }

        private static string RandomString(string chars, int length)
        {
            var res = new char[length];
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            for (int i = 0; i < length; i++)
            {
                res[i] = chars[bytes[i] % chars.Length];
            }
            return new string(res);
        }
    }
}
