using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Api.Controllers
{
    [ApiController]
    public class SkinApiController : ControllerBase
    {
        private readonly SkinStorageService _storage;

        public SkinApiController(SkinStorageService storage)
        {
            _storage = storage;
        }

        [HttpGet("textures/{hash}")]
        [HttpGet("textures/{hash}.png")]
        [ResponseCache(Duration = 31536000, Location = ResponseCacheLocation.Any)]
        public IActionResult GetTextureByHash(string hash)
        {
            var clean = Path.GetFileNameWithoutExtension(hash);
            var bytes = _storage.GetTextureBytesByHash(clean);
            if (bytes == null || bytes.Length == 0) return NotFound();
            Response.Headers["Cache-Control"] = "public, max-age=31536000, immutable";
            return File(bytes, "image/png");
        }

        [HttpGet("api/skin/{username}.png")]
        public IActionResult GetSkin(string username)
        {
            var clean = Path.GetFileNameWithoutExtension(username);
            var bytes = _storage.GetSkinBytes(clean);
            if (bytes.Length == 0) return NotFound();
            return File(bytes, "image/png");
        }

        [HttpGet("api/skin/{username}.json")]
        public IActionResult GetSkinMeta(string username)
        {
            var clean = Path.GetFileNameWithoutExtension(username);
            var model = _storage.GetSkinModel(clean);
            var hasCape = _storage.HasCape(clean);
            return Ok(new
            {
                username = clean,
                model = model,
                skinUrl = _storage.GetSkinUrl(clean),
                capeUrl = _storage.GetCapeUrl(clean),
                hasCape = hasCape,
                equippedCapeId = _storage.DetectEquippedCapeId(clean)
            });
        }

        [HttpGet("api/cape/{username}.png")]
        public IActionResult GetCape(string username)
        {
            var clean = Path.GetFileNameWithoutExtension(username);
            var bytes = _storage.GetCapeBytes(clean);
            if (bytes == null || bytes.Length == 0) return NotFound();
            return File(bytes, "image/png");
        }

        [HttpPost("api/skin/upload")]
        public async Task<IActionResult> UploadSkinFromLauncher([FromForm] string username, [FromForm] string? model, IFormFile? file)
        {
            if (string.IsNullOrWhiteSpace(username) || file == null || file.Length == 0)
                return BadRequest(new { error = "Missing file or username." });

            var cleanUser = Path.GetFileNameWithoutExtension(username);
            var cleanModel = string.Equals(model, "slim", StringComparison.OrdinalIgnoreCase) ? "slim" : "classic";

            var ok = await _storage.SaveSkinAsync(cleanUser, file, cleanModel);
            if (!ok) return BadRequest(new { error = "Failed to save skin." });

            return Ok(new { success = true, message = "Skin uploaded successfully." });
        }

        [HttpPost("api/skin/reset")]
        public IActionResult ResetSkinFromLauncher([FromForm] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { error = "Missing username." });

            var cleanUser = Path.GetFileNameWithoutExtension(username);
            _storage.ResetSkin(cleanUser);
            return Ok(new { success = true, message = "Skin reset to default." });
        }

        [HttpPost("api/skin/model")]
        public IActionResult UpdateSkinModel([FromForm] string username, [FromForm] string model)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(model))
                return BadRequest(new { error = "Missing username or model." });

            var cleanUser = Path.GetFileNameWithoutExtension(username);
            var cleanModel = string.Equals(model, "slim", StringComparison.OrdinalIgnoreCase) ? "slim" : "classic";
            _storage.SetSkinModel(cleanUser, cleanModel);
            return Ok(new { success = true, model = cleanModel });
        }

        [HttpPost("api/cape/upload")]
        public async Task<IActionResult> UploadCapeFromLauncher([FromForm] string username, IFormFile? file)
        {
            if (string.IsNullOrWhiteSpace(username) || file == null || file.Length == 0)
                return BadRequest(new { error = "Missing file or username." });

            var cleanUser = Path.GetFileNameWithoutExtension(username);
            var ok = await _storage.SaveCustomCapeAsync(cleanUser, file);
            if (!ok) return BadRequest(new { error = "Failed to save cape." });

            return Ok(new { success = true, message = "Cape uploaded successfully." });
        }

        [HttpGet("api/capes/catalog")]
        public IActionResult GetCapesCatalog()
        {
            return Ok(_storage.GetCapesCatalog());
        }

        public class EquipCapeRequest
        {
            public string Username { get; set; } = string.Empty;
            public string CapeId { get; set; } = string.Empty;
        }

        [HttpPost("api/cape/equip")]
        public IActionResult EquipCape([FromBody] EquipCapeRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.CapeId))
                return BadRequest(new { error = "Missing username or capeId." });

            var cleanUser = Path.GetFileNameWithoutExtension(req.Username);
            var ok = _storage.EquipLauncherCape(cleanUser, req.CapeId);
            if (!ok) return BadRequest(new { error = "Cape not found or failed to equip." });

            return Ok(new { success = true, message = "Cape equipped successfully." });
        }

        [HttpPost("api/cape/reset")]
        public IActionResult ResetCapeFromLauncher([FromForm] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { error = "Missing username." });

            var cleanUser = Path.GetFileNameWithoutExtension(username);
            _storage.RemoveCape(cleanUser);
            return Ok(new { success = true, message = "Cape removed." });
        }

        [HttpGet("api/csl/{username}.json")]
        public IActionResult GetCslJson(string username)
        {
            var clean = Path.GetFileNameWithoutExtension(username);
            var isSlim = _storage.GetSkinModel(clean) == "slim";

            var skins = new Dictionary<string, string>
            {
                [isSlim ? "slim" : "default"] = $"{clean}.png"
            };

            var result = new Dictionary<string, object>
            {
                ["username"] = clean,
                ["skins"] = skins
            };

            if (_storage.HasCape(clean))
            {
                result["cape"] = $"cape_{clean}.png";
            }

            return Ok(result);
        }

        [HttpGet("api/csl/textures/{filename}")]
        public IActionResult GetCslTexture(string filename)
        {
            var clean = Path.GetFileNameWithoutExtension(filename);
            if (clean.StartsWith("cape_", StringComparison.OrdinalIgnoreCase))
            {
                var capeUser = clean.Substring(5);
                var capeBytes = _storage.GetCapeBytes(capeUser);
                if (capeBytes != null && capeBytes.Length > 0)
                    return File(capeBytes, "image/png");
            }

            var skinBytes = _storage.GetSkinBytes(clean);
            if (skinBytes.Length > 0)
                return File(skinBytes, "image/png");

            return NotFound();
        }
    }
}
