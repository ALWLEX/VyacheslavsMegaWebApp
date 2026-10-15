using System;
using System.Collections.Generic;

namespace VyacheslavsMegaWebApp.Models
{
    public class CapeItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PreviewUrl { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public bool IsEquipped { get; set; }
        public bool IsOfficial { get; set; }
    }

    public class LauncherSessionInfo
    {
        public string SessionKey { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsActive => ExpiresAt > DateTime.UtcNow;
    }

    public class ProfileViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string DisplayId => string.IsNullOrEmpty(UserId) ? "" : $"#VEC-{UserId.Replace("-", "").Substring(0, Math.Min(6, UserId.Replace("-", "").Length)).ToUpper()}";
        public string RoleBadge { get; set; } = "Игрок VEC";

        public string SkinUrl { get; set; } = string.Empty;
        public string SkinModel { get; set; } = "classic";
        public bool HasCustomSkin { get; set; }

        public string? CapeUrl { get; set; }
        public bool HasCape { get; set; }
        public string? EquippedCapeId { get; set; }

        public string AvatarUrl { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string MinecraftUsername { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public int FriendsCount { get; set; } = 0;

        public string BannerUrl { get; set; } = "/img/backgrounds/bg1.webp";
        public bool HasCustomBanner { get; set; }

        public List<CapeItemViewModel> AvailableCapes { get; set; } = new();
        public List<LauncherSessionInfo> LauncherSessions { get; set; } = new();
        public List<FriendItemViewModel> Friends { get; set; } = new();
        public List<FriendItemViewModel> IncomingRequests { get; set; } = new();
        public List<FriendItemViewModel> OutgoingRequests { get; set; } = new();

        public ChangePasswordViewModel ChangePassword { get; set; } = new();
    }
}
