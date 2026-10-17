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
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminAreaPolicy")]
    public class ClientBuildsEditorController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ModrinthService _modrinth;

        public ClientBuildsEditorController(AppDbContext db, IWebHostEnvironment env, ModrinthService modrinth)
        {
            _db = db;
            _env = env;
            _modrinth = modrinth;
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

        [HttpGet]
        public async Task<IActionResult> LookupModrinth(string query, string? category)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { error = "Укажите ссылку или ID мода на Modrinth" });
            }

            var info = await _modrinth.FetchModrinthInfoAsync(query, category ?? "Core");
            if (info == null)
            {
                return NotFound(new { error = "Проект не найден на Modrinth или не содержит подходящих файлов для Fabric" });
            }

            return Ok(new
            {
                slug = info.Slug,
                title = info.Title,
                description = info.Description,
                iconUrl = info.IconUrl,
                category = info.Category,
                fileName = info.FileName,
                downloadUrl = info.DownloadUrl,
                fileSizeBytes = info.FileSizeBytes,
                sha256 = info.Sha256,
                relativePath = info.RelativePath
            });
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
                    Category = "Optimization",
                    SourceType = "Local"
                });
            }

            var item = await _db.ClientBuildFiles.FindAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(item.SourceType))
            {
                item.SourceType = item.DownloadUrl?.StartsWith("http", StringComparison.OrdinalIgnoreCase) == true ? "Modrinth" : "Local";
            }

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> ModDetails(ClientBuildFile model, IFormFile? modFile, string slug = "vec-fabric-1-21-11")
        {
            ViewBag.Slug = slug;

            var isModrinth = string.Equals(model.SourceType, "Modrinth", StringComparison.OrdinalIgnoreCase);

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

            if (isModrinth)
            {
                if (string.IsNullOrWhiteSpace(model.ModrinthSlug) && string.IsNullOrWhiteSpace(model.DownloadUrl))
                {
                    ModelState.AddModelError("", "Для компонента из Modrinth необходимо указать ID или ссылку на проект.");
                    return View(model);
                }

                if (!string.IsNullOrWhiteSpace(model.ModrinthSlug) && (string.IsNullOrWhiteSpace(model.DownloadUrl) || model.Id == 0))
                {
                    var info = await _modrinth.FetchModrinthInfoAsync(model.ModrinthSlug, model.Category);
                    if (info == null && string.IsNullOrWhiteSpace(model.DownloadUrl))
                    {
                        ModelState.AddModelError("", "Не удалось получить данные с Modrinth API. Проверьте ID/ссылку.");
                        return View(model);
                    }

                    if (info != null)
                    {
                        model.DownloadUrl = info.DownloadUrl;
                        model.FileName = info.FileName;
                        model.RelativePath = info.RelativePath;
                        model.Sha256 = info.Sha256;
                        model.FileSizeBytes = info.FileSizeBytes;
                        if (string.IsNullOrEmpty(model.IconUrl)) model.IconUrl = info.IconUrl;
                        if (string.IsNullOrWhiteSpace(model.DisplayName)) model.DisplayName = info.Title;
                        if (string.IsNullOrWhiteSpace(model.Description)) model.Description = info.Description;
                    }
                }

                if (string.IsNullOrWhiteSpace(model.FileName))
                {
                    model.FileName = $"{ModrinthService.ExtractSlug(model.ModrinthSlug ?? "component") ?? "component"}.jar";
                }

                if (string.IsNullOrWhiteSpace(model.RelativePath))
                {
                    model.RelativePath = $"{subFolder}/{model.FileName}";
                }
            }
            else
            {
                model.SourceType = "Local";

                if (model.Id == 0 && (modFile == null || modFile.Length == 0))
                {
                    ModelState.AddModelError("", "Для добавления локального компонента необходимо выбрать файл (.jar или .zip).");
                    return View(model);
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
                existing.SourceType = model.SourceType;
                existing.ModrinthSlug = model.ModrinthSlug;
                existing.IconUrl = model.IconUrl;

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
                if (item.DownloadUrl == null || !item.DownloadUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                    var rel = item.RelativePath.Replace('/', Path.DirectorySeparatorChar);
                    var targetPath = Path.Combine(root, "builds", item.BuildSlug, rel);
                    if (System.IO.File.Exists(targetPath))
                    {
                        try { System.IO.File.Delete(targetPath); } catch { }
                    }
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
