using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaWebApp.Data;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.Entities.Identity;

namespace VyacheslavsMegaWebApp.Api.Controllers
{
    [ApiController]
    [Route("api/friends")]
    public class FriendsApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<VmsIdentityUser> _userManager;

        public FriendsApiController(AppDbContext db, UserManager<VmsIdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public class FriendDto
        {
            public int FriendshipId { get; set; }
            public string UserId { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string AvatarUrl { get; set; } = string.Empty;
            public bool IsOnline { get; set; }
            public string Status { get; set; } = "Accepted";
            public DateTime CreatedAt { get; set; }
        }

        public class FriendRequestDto
        {
            public int FriendshipId { get; set; }
            public string RequesterId { get; set; } = string.Empty;
            public string RequesterUsername { get; set; } = string.Empty;
            public string AvatarUrl { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }

        public class FriendsListResponse
        {
            public List<FriendDto> Friends { get; set; } = new();
            public List<FriendRequestDto> IncomingRequests { get; set; } = new();
            public List<FriendRequestDto> OutgoingRequests { get; set; } = new();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetFriendsList([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { error = "Username required." });

            var cleanUser = Path.GetFileNameWithoutExtension(username).Trim();
            var user = await _userManager.FindByNameAsync(cleanUser);
            if (user == null)
                return NotFound(new { error = "User not found." });

            var accepted = await _db.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Where(f => (f.RequesterId == user.Id || f.AddresseeId == user.Id) && f.Status == FriendshipStatus.Accepted)
                .ToListAsync();

            var incoming = await _db.Friendships
                .Include(f => f.Requester)
                .Where(f => f.AddresseeId == user.Id && f.Status == FriendshipStatus.Pending)
                .ToListAsync();

            var outgoing = await _db.Friendships
                .Include(f => f.Addressee)
                .Where(f => f.RequesterId == user.Id && f.Status == FriendshipStatus.Pending)
                .ToListAsync();

            var activeSessionUserIds = await _db.LauncherSessions
                .Where(s => s.ExpiresAt > DateTime.UtcNow)
                .Select(s => s.UserId)
                .Distinct()
                .ToListAsync();

            var activeSessionSet = new HashSet<string>(activeSessionUserIds);

            var response = new FriendsListResponse();

            foreach (var f in accepted)
            {
                var isRequester = f.RequesterId == user.Id;
                var other = isRequester ? f.Addressee : f.Requester;
                if (other == null) continue;

                var otherName = other.UserName ?? "Player";
                response.Friends.Add(new FriendDto
                {
                    FriendshipId = f.Id,
                    UserId = other.Id,
                    Username = otherName,
                    AvatarUrl = $"/api/skin/{otherName}.png",
                    IsOnline = activeSessionSet.Contains(other.Id),
                    Status = "Accepted",
                    CreatedAt = f.CreatedAt
                });
            }

            foreach (var req in incoming)
            {
                if (req.Requester == null) continue;
                var rName = req.Requester.UserName ?? "Player";
                response.IncomingRequests.Add(new FriendRequestDto
                {
                    FriendshipId = req.Id,
                    RequesterId = req.RequesterId,
                    RequesterUsername = rName,
                    AvatarUrl = $"/api/skin/{rName}.png",
                    CreatedAt = req.CreatedAt
                });
            }

            foreach (var req in outgoing)
            {
                if (req.Addressee == null) continue;
                var aName = req.Addressee.UserName ?? "Player";
                response.OutgoingRequests.Add(new FriendRequestDto
                {
                    FriendshipId = req.Id,
                    RequesterId = req.RequesterId,
                    RequesterUsername = aName,
                    AvatarUrl = $"/api/skin/{aName}.png",
                    CreatedAt = req.CreatedAt
                });
            }

            return Ok(response);
        }

        public class SendRequestModel
        {
            public string Requester { get; set; } = string.Empty;
            public string Target { get; set; } = string.Empty;
        }

        [HttpPost("request")]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Requester) || string.IsNullOrWhiteSpace(model.Target))
                return BadRequest(new { error = "Requester and Target required." });

            var cleanRequester = Path.GetFileNameWithoutExtension(model.Requester).Trim();
            var cleanTarget = Path.GetFileNameWithoutExtension(model.Target).Trim();

            if (cleanRequester.Equals(cleanTarget, StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { error = "Cannot add yourself as a friend." });

            var reqUser = await _userManager.FindByNameAsync(cleanRequester);
            var tarUser = await _userManager.FindByNameAsync(cleanTarget);

            if (reqUser == null || tarUser == null)
                return NotFound(new { error = "User not found." });

            var existing = await _db.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.RequesterId == reqUser.Id && f.AddresseeId == tarUser.Id) ||
                    (f.RequesterId == tarUser.Id && f.AddresseeId == reqUser.Id));

            if (existing != null)
            {
                if (existing.Status == FriendshipStatus.Accepted)
                    return BadRequest(new { error = "Already friends." });

                if (existing.Status == FriendshipStatus.Pending)
                {
                    if (existing.RequesterId == tarUser.Id)
                    {
                        existing.Status = FriendshipStatus.Accepted;
                        existing.UpdatedAt = DateTime.UtcNow;
                        await _db.SaveChangesAsync();
                        return Ok(new { success = true, status = "Accepted", message = "Friend request accepted." });
                    }
                    return BadRequest(new { error = "Friend request already sent." });
                }

                existing.Status = FriendshipStatus.Pending;
                existing.RequesterId = reqUser.Id;
                existing.AddresseeId = tarUser.Id;
                existing.CreatedAt = DateTime.UtcNow;
                existing.UpdatedAt = null;
                await _db.SaveChangesAsync();
                return Ok(new { success = true, status = "Pending", message = "Friend request sent." });
            }

            var friendship = new Friendship
            {
                RequesterId = reqUser.Id,
                AddresseeId = tarUser.Id,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _db.Friendships.Add(friendship);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, status = "Pending", message = "Friend request sent." });
        }

        public class RespondModel
        {
            public int FriendshipId { get; set; }
            public string Username { get; set; } = string.Empty;
            public bool Accept { get; set; }
        }

        [HttpPost("respond")]
        public async Task<IActionResult> RespondToFriendRequest([FromBody] RespondModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                return BadRequest(new { error = "Username required." });

            var cleanUser = Path.GetFileNameWithoutExtension(model.Username).Trim();
            var user = await _userManager.FindByNameAsync(cleanUser);
            if (user == null)
                return NotFound(new { error = "User not found." });

            var friendship = await _db.Friendships.FirstOrDefaultAsync(f => f.Id == model.FriendshipId);
            if (friendship == null)
                return NotFound(new { error = "Friendship not found." });

            if (friendship.AddresseeId != user.Id)
                return Forbid();

            if (model.Accept)
            {
                friendship.Status = FriendshipStatus.Accepted;
                friendship.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return Ok(new { success = true, status = "Accepted" });
            }
            else
            {
                _db.Friendships.Remove(friendship);
                await _db.SaveChangesAsync();
                return Ok(new { success = true, status = "Declined" });
            }
        }

        public class RemoveFriendModel
        {
            public string Username { get; set; } = string.Empty;
            public string Target { get; set; } = string.Empty;
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Target))
                return BadRequest(new { error = "Username and Target required." });

            var cleanUser = Path.GetFileNameWithoutExtension(model.Username).Trim();
            var cleanTarget = Path.GetFileNameWithoutExtension(model.Target).Trim();

            var u1 = await _userManager.FindByNameAsync(cleanUser);
            var u2 = await _userManager.FindByNameAsync(cleanTarget);

            if (u1 == null || u2 == null)
                return NotFound(new { error = "User not found." });

            var friendship = await _db.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == u1.Id && f.AddresseeId == u2.Id) ||
                (f.RequesterId == u2.Id && f.AddresseeId == u1.Id));

            if (friendship == null)
                return NotFound(new { error = "Friendship not found." });

            _db.Friendships.Remove(friendship);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "Friend removed." });
        }

        public class SearchUserDto
        {
            public string Id { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string AvatarUrl { get; set; } = string.Empty;
            public bool IsFriend { get; set; }
            public bool IsPending { get; set; }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string q, [FromQuery] string? current)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Ok(new List<SearchUserDto>());

            var query = q.Trim().ToLower();
            var currentUser = !string.IsNullOrWhiteSpace(current) ? await _userManager.FindByNameAsync(current.Trim()) : null;

            var matchedUsers = await _userManager.Users
                .Where(u => u.UserName != null && u.UserName.ToLower().Contains(query))
                .Take(15)
                .ToListAsync();

            var friendIds = new HashSet<string>();
            var pendingIds = new HashSet<string>();

            if (currentUser != null)
            {
                var friendships = await _db.Friendships
                    .Where(f => f.RequesterId == currentUser.Id || f.AddresseeId == currentUser.Id)
                    .ToListAsync();

                foreach (var f in friendships)
                {
                    var otherId = f.RequesterId == currentUser.Id ? f.AddresseeId : f.RequesterId;
                    if (f.Status == FriendshipStatus.Accepted)
                        friendIds.Add(otherId);
                    else if (f.Status == FriendshipStatus.Pending)
                        pendingIds.Add(otherId);
                }
            }

            var result = new List<SearchUserDto>();
            foreach (var u in matchedUsers)
            {
                if (currentUser != null && u.Id == currentUser.Id) continue;
                var uName = u.UserName ?? "Player";
                result.Add(new SearchUserDto
                {
                    Id = u.Id,
                    Username = uName,
                    AvatarUrl = $"/api/skin/{uName}.png",
                    IsFriend = friendIds.Contains(u.Id),
                    IsPending = pendingIds.Contains(u.Id)
                });
            }

            return Ok(result);
        }
    }
}
