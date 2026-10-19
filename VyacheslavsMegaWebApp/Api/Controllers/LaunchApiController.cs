using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaWebApp.Data;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Api.Controllers
{
    [ApiController]
    public class LaunchApiController : ControllerBase
    {
        private readonly LaunchTicketService _ticketService;
        private readonly AppDbContext _db;
        private readonly UserManager<VmsIdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public LaunchApiController(
            LaunchTicketService ticketService,
            AppDbContext db,
            UserManager<VmsIdentityUser> userManager,
            IWebHostEnvironment env)
        {
            _ticketService = ticketService;
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        public class RequestTicketModel
        {
            public string Username { get; set; } = string.Empty;
            public string SessionKey { get; set; } = string.Empty;
            public string Hwid { get; set; } = string.Empty;
        }

        [HttpPost("api/launch/request-ticket")]
        public async Task<IActionResult> RequestTicket([FromBody] RequestTicketModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                return BadRequest(new { error = "Username required." });

            var cleanUser = Path.GetFileNameWithoutExtension(model.Username).Trim();
            var user = await _userManager.FindByNameAsync(cleanUser);
            if (user == null)
                return NotFound(new { error = "User not found." });

            if (!string.IsNullOrWhiteSpace(model.SessionKey))
            {
                var keyHash = SHA256.HashData(Encoding.UTF8.GetBytes(model.SessionKey));
                var session = await _db.LauncherSessions.FirstOrDefaultAsync(s => s.SessionKeyHash == keyHash || s.SessionKey == model.SessionKey);
                if (session == null || session.ExpiresAt < DateTime.UtcNow || session.UserId != user.Id)
                    return Unauthorized(new { error = "Invalid or expired session." });
            }

            var ticket = _ticketService.CreateTicket(cleanUser, model.Hwid ?? string.Empty);
            return Ok(new
            {
                success = true,
                ticket = ticket.TicketId,
                username = ticket.Username,
                expiresAt = ticket.ExpiresAt
            });
        }

        public class VerifyTicketModel
        {
            public string Ticket { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
        }

        [HttpPost("api/launch/verify-ticket")]
        public IActionResult VerifyTicket([FromBody] VerifyTicketModel model)
        {
            var valid = _ticketService.ValidateAndConsumeTicket(model.Ticket, model.Username);
            if (!valid)
                return BadRequest(new { valid = false, error = "Invalid or expired ticket." });

            return Ok(new { valid = true, message = "Ticket verified." });
        }

        [HttpGet("api/launch/check-active/{username}")]
        public IActionResult CheckActiveTicket(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { active = false });

            var active = _ticketService.HasActiveTicketForUser(username.Trim());
            return Ok(new { active, username = username.Trim() });
        }

        [HttpGet("api/launcher/version")]
        public IActionResult GetLauncherVersion()
        {
            return Ok(new
            {
                version = "3.0.0",
                minVersion = "2.0.0",
                downloadUrl = "/downloads/VECLauncher.exe",
                changelog = "VEC Ecosystem 3.0: Unified Yggdrasil API, Friends Tab, Launch Tickets, Wardrobe Sync."
            });
        }

        public class ManifestFileEntry
        {
            public string Path { get; set; } = string.Empty;
            public string FileName { get; set; } = string.Empty;
            public string Sha256 { get; set; } = string.Empty;
            public long Size { get; set; }
            public bool IsRequired { get; set; } = true;
            public string DownloadUrl { get; set; } = string.Empty;
            public string Category { get; set; } = "Core";
            public string DisplayName { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string? IconUrl { get; set; }
            public string? BannerUrl { get; set; }
            public string? ModrinthSlug { get; set; }
            public string? Source { get; set; }
        }

        [HttpGet("api/builds/{slug}/manifest")]
        public async Task<IActionResult> GetBuildManifest(string slug)
        {
            var cleanSlug = string.IsNullOrWhiteSpace(slug) ? "vec-fabric-1-21-11" : slug.Trim().ToLowerInvariant();
            var files = await GetManifestDataAsync(cleanSlug);
            return Ok(new
            {
                build = cleanSlug,
                version = "1.21.11",
                files = files
            });
        }

        [HttpGet("api/builds/{slug}/mods")]
        public async Task<IActionResult> GetBuildMods(string slug)
        {
            var cleanSlug = string.IsNullOrWhiteSpace(slug) ? "vec-fabric-1-21-11" : slug.Trim().ToLowerInvariant();
            var files = await GetManifestDataAsync(cleanSlug);
            var mods = files.Where(f => f.Path.StartsWith("mods/", StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(new { build = cleanSlug, files = files, mods = mods });
        }

        private async Task<List<ManifestFileEntry>> GetManifestDataAsync(string cleanSlug)
        {
            var dbFiles = await _db.ClientBuildFiles.Where(f => f.BuildSlug == cleanSlug).ToListAsync();

            if (dbFiles.Count == 0)
            {
                var defaults = new List<ClientBuildFile>
                {
                    new() { BuildSlug = cleanSlug, RelativePath = "mods/fabric-api.jar", FileName = "fabric-api.jar", Sha256 = "", FileSizeBytes = 0, IsRequired = true, Category = "Core", DisplayName = "Fabric API", Description = "Базовый модуль хуков и событий Fabric 1.21.11", SourceType = "Local" },
                    new() { BuildSlug = cleanSlug, RelativePath = "mods/voicechat-fabric.jar", FileName = "voicechat-fabric.jar", Sha256 = "", FileSizeBytes = 0, IsRequired = true, Category = "Core", DisplayName = "Simple Voice Chat", Description = "Позиционный голосовой 3D чат сервера VEC", SourceType = "Local" },
                    new() { BuildSlug = cleanSlug, RelativePath = "mods/sodium-fabric.jar", FileName = "sodium-fabric.jar", Sha256 = "", FileSizeBytes = 0, IsRequired = true, Category = "Optimization", DisplayName = "Sodium", Description = "Движок рендеринга и многократного повышения FPS", SourceType = "Local" },
                    new() { BuildSlug = cleanSlug, RelativePath = "mods/iris-fabric.jar", FileName = "iris-fabric.jar", Sha256 = "", FileSizeBytes = 0, IsRequired = false, Category = "Visual", DisplayName = "Iris Shaders", Description = "Современный движок ультра-шейдеров", SourceType = "Local" },
                    new() { BuildSlug = cleanSlug, RelativePath = "mods/modmenu-fabric.jar", FileName = "modmenu-fabric.jar", Sha256 = "", FileSizeBytes = 0, IsRequired = false, Category = "Utility", DisplayName = "Mod Menu", Description = "Удобный список и настройки модов в главном меню", SourceType = "Local" },
                    new() { BuildSlug = cleanSlug, RelativePath = "mods/appleskin-fabric.jar", FileName = "appleskin-fabric.jar", Sha256 = "", FileSizeBytes = 0, IsRequired = false, Category = "Utility", DisplayName = "AppleSkin", Description = "Показ насыщения еды и очков голода", SourceType = "Local" }
                };

                _db.ClientBuildFiles.AddRange(defaults);
                await _db.SaveChangesAsync();
                dbFiles = defaults;
            }

            var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var buildDir = Path.Combine(root, "builds", cleanSlug);

            var result = new List<ManifestFileEntry>();
            using var sha = SHA256.Create();

            foreach (var dbf in dbFiles)
            {
                var localFile = Path.Combine(buildDir, dbf.RelativePath.Replace('/', Path.DirectorySeparatorChar));
                var fileSha = dbf.Sha256;
                var fileSize = dbf.FileSizeBytes;

                if (System.IO.File.Exists(localFile))
                {
                    var fi = new FileInfo(localFile);
                    fileSize = fi.Length;
                    using var stream = System.IO.File.OpenRead(localFile);
                    fileSha = BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }

                result.Add(new ManifestFileEntry
                {
                    Path = dbf.RelativePath.Replace('\\', '/'),
                    FileName = string.IsNullOrWhiteSpace(dbf.FileName) ? Path.GetFileName(dbf.RelativePath) : dbf.FileName,
                    Sha256 = fileSha,
                    Size = fileSize,
                    IsRequired = dbf.IsRequired,
                    DownloadUrl = !string.IsNullOrWhiteSpace(dbf.DownloadUrl) ? dbf.DownloadUrl : $"/api/builds/{cleanSlug}/file/{dbf.RelativePath.Replace('\\', '/')}",
                    Category = dbf.Category ?? "Core",
                    DisplayName = dbf.DisplayName ?? Path.GetFileNameWithoutExtension(dbf.RelativePath),
                    Description = dbf.Description ?? string.Empty,
                    IconUrl = dbf.IconUrl,
                    BannerUrl = dbf.BannerUrl,
                    ModrinthSlug = dbf.ModrinthSlug,
                    Source = dbf.SourceType ?? (dbf.DownloadUrl?.StartsWith("http", StringComparison.OrdinalIgnoreCase) == true ? "Modrinth" : "Local")
                });
            }

            return result;
        }

        [HttpGet("api/builds/{slug}/file/{**filePath}")]
        public IActionResult GetBuildFile(string slug, string filePath)
        {
            if (string.IsNullOrWhiteSpace(slug) || string.IsNullOrWhiteSpace(filePath))
                return BadRequest();

            var cleanSlug = Path.GetFileName(slug).ToLowerInvariant();
            var cleanPath = filePath.Replace('\\', '/').TrimStart('/');

            var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var fullPath = Path.GetFullPath(Path.Combine(root, "builds", cleanSlug, cleanPath));

            var expectedBase = Path.GetFullPath(Path.Combine(root, "builds", cleanSlug));
            if (!fullPath.StartsWith(expectedBase, StringComparison.OrdinalIgnoreCase))
                return Forbid();

            if (!System.IO.File.Exists(fullPath))
                return NotFound(new { error = "Build file not found on server." });

            var fileName = Path.GetFileName(fullPath);
            return PhysicalFile(fullPath, "application/octet-stream", fileName);
        }

        [HttpGet("api/launcher/download")]
        public IActionResult DownloadLauncher()
        {
            var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var path = Path.Combine(root, "downloads", "VECLauncher.exe");
            if (!System.IO.File.Exists(path))
            {
                return NotFound(new { error = "Launcher binary not found on server." });
            }
            return PhysicalFile(path, "application/vnd.microsoft.portable-executable", "VECLauncher.exe");
        }
    }
}
