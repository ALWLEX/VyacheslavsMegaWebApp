using System.Collections.Generic;

namespace VyacheslavsMegaWebApp.Models
{
    public class MojangCapeInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class MinecraftLicenseResult
    {
        public bool Success { get; set; } = true;
        public bool HasLicense { get; set; }
        public bool HasMinecraftLicense => HasLicense;
        public string? ErrorMessage { get; set; }
        public string Gamertag { get; set; } = string.Empty;
        public string Uuid { get; set; } = string.Empty;
        public List<MojangCapeInfo> Capes { get; set; } = new();
        public string? SkinUrl { get; set; }
        public string? SkinModel { get; set; }
    }
}
