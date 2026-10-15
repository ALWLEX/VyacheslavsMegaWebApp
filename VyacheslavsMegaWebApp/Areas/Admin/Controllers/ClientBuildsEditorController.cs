using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaWebApp.Data;
using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminAreaPolicy")]
    public class ClientBuildsEditorController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ClientBuildsEditorController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        private string GetBuildsDir(string slug, string folder = "mods")
        {
            var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var dir = Path.Combine(root, "builds", slug, folder);
            Directory.CreateDirectory(dir);
            return dir;
        }

        public async Task<IActionResult> Index(string slug = "vec-fabric-1-21-11")
        {
            ViewBag.Slug = slug;
            var files = await _db.ClientBuildFiles
                .Where(f => f.BuildSlug == slug)
                .OrderBy(f => f.Category)
                .ThenBy(f => f.DisplayName)
                .ToListAsync();

            return View(files);
        }

        public async Task<IActionResult> ModDetails(int? id, string slug = "vec-fabric-1-21-11")
        {
            ViewBag.Slug = slug;
            if (id == null)
            {
                return View(new ClientBuildFile
                {
                    BuildSlug = slug,
                    IsRequired = true,
                    Category = "Optimization"
                });
            }

            var item = await _db.ClientBuildFiles.FindAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> ModDetails(ClientBuildFile model, IFormFile? modFile, string slug = "vec-fabric-1-21-11")
        {
            ViewBag.Slug = slug;

            if (model.Id == 0 && (modFile == null || modFile.Length == 0))
            {
                ModelState.AddModelError("", "Для добавления компонента необходимо выбрать файл (.jar или .zip).");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Category))
            {
                model.Category = "Core";
            }

            var subFolder = "mods";
            if (string.Equals(model.Category, "Shaders", StringComparison.OrdinalIgnoreCase))
            {
                subFolder = "shaderpacks";
            }
            else if (string.Equals(model.Category, "Resourcepacks", StringComparison.OrdinalIgnoreCase))
            {
                subFolder = "resourcepacks";
            }

            if (modFile != null && modFile.Length > 0)
            {
                var ext = Path.GetExtension(modFile.FileName);
                if (!string.Equals(ext, ".jar", StringComparison.OrdinalIgnoreCase) && !string.Equals(ext, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("", "Разрешена загрузка только файлов с расширением .jar или .zip.");
                    return View(model);
                }

                var cleanFileName = Path.GetFileName(modFile.FileName);
                var targetDir = GetBuildsDir(slug, subFolder);
                var targetPath = Path.Combine(targetDir, cleanFileName);

                using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
                {
                    await modFile.CopyToAsync(fileStream);
                }

                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(targetPath);
                string sha256;
                using (var sha = SHA256.Create())
                {
                    sha256 = Convert.ToHexString(sha.ComputeHash(fileBytes)).ToLowerInvariant();
                }

                model.FileName = cleanFileName;
                model.RelativePath = $"{subFolder}/{cleanFileName}";
                model.Sha256 = sha256;
                model.FileSizeBytes = fileBytes.LongLength;
                model.DownloadUrl = $"/api/builds/{slug}/file/{subFolder}/{cleanFileName}";
            }

            model.BuildSlug = slug;
            if (string.IsNullOrWhiteSpace(model.DisplayName))
            {
                model.DisplayName = model.FileName ?? "Компонент сборки";
            }

            if (model.Id == 0)
            {
                _db.ClientBuildFiles.Add(model);
            }
            else
            {
                var existing = await _db.ClientBuildFiles.FindAsync(model.Id);
                if (existing == null)
                {
                    return NotFound();
                }

                existing.DisplayName = model.DisplayName;
                existing.Description = model.Description ?? string.Empty;
                existing.Category = model.Category;
                existing.IsRequired = model.IsRequired;

                if (!string.IsNullOrEmpty(model.FileName))
                {
                    existing.FileName = model.FileName;
                    existing.RelativePath = model.RelativePath;
                    existing.Sha256 = model.Sha256;
                    existing.FileSizeBytes = model.FileSizeBytes;
                    existing.DownloadUrl = model.DownloadUrl;
                }
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Компонент «{model.DisplayName}» успешно сохранен.";
            return RedirectToAction(nameof(Index), new { slug });
        }

        public async Task<IActionResult> DeleteMod(int id, string slug = "vec-fabric-1-21-11")
        {
            var item = await _db.ClientBuildFiles.FindAsync(id);
            if (item != null)
            {
                var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                var rel = item.RelativePath.Replace('/', Path.DirectorySeparatorChar);
                var targetPath = Path.Combine(root, "builds", item.BuildSlug, rel);
                if (System.IO.File.Exists(targetPath))
                {
                    try { System.IO.File.Delete(targetPath); } catch { }
                }

                _db.ClientBuildFiles.Remove(item);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Компонент «{item.DisplayName}» удален из сборки.";
            }

            return RedirectToAction(nameof(Index), new { slug });
        }

        public async Task<IActionResult> ToggleRequired(int id, string slug = "vec-fabric-1-21-11")
        {
            var item = await _db.ClientBuildFiles.FindAsync(id);
            if (item != null)
            {
                item.IsRequired = !item.IsRequired;
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Статус обязательности для «{item.DisplayName}» изменен на: {(item.IsRequired ? "Обязательный" : "Опциональный")}.";
            }

            return RedirectToAction(nameof(Index), new { slug });
        }
    }
}
