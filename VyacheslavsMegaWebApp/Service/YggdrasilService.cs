using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using VyacheslavsMegaWebApp.Api.Models;

namespace VyacheslavsMegaWebApp.Service
{
    public sealed class YggdrasilService
    {
        private readonly RSA _rsa;
        private readonly string _serverPublicDomain;
        private readonly string _keyFilePath;
        private readonly SkinStorageService _skinStorage;

        public YggdrasilService(IWebHostEnvironment env, IConfiguration config, SkinStorageService skinStorage)
        {
            _skinStorage = skinStorage;
            _serverPublicDomain = config["Server:PublicUrl"] ?? "http://localhost:5044";
            _keyFilePath = Path.Combine(env.ContentRootPath, "App_Data", "yggdrasil_key.pem");
            _rsa = LoadOrGenerateRsaKey();
        }

        private RSA LoadOrGenerateRsaKey()
        {
            var rsa = RSA.Create(2048);
            try
            {
                var dir = Path.GetDirectoryName(_keyFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (File.Exists(_keyFilePath))
                {
                    var pem = File.ReadAllText(_keyFilePath);
                    rsa.ImportFromPem(pem);
                }
                else
                {
                    var pem = rsa.ExportPkcs8PrivateKeyPem();
                    File.WriteAllText(_keyFilePath, pem);
                }
            }
            catch
            {
            }

            return rsa;
        }

        public string GetPublicKeyPem()
        {
            return _rsa.ExportSubjectPublicKeyInfoPem();
        }

        public PropertyDto BuildTexturesProperty(string username, string userId, string? baseUrl = null)
        {
            var domain = (baseUrl ?? _serverPublicDomain).TrimEnd('/');
            var textures = new Dictionary<string, object>();
            var model = _skinStorage.GetSkinModel(username);
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var skinHash = _skinStorage.GetSkinHash(username);
            var skinUrl = !string.IsNullOrEmpty(skinHash)
                ? $"{domain}/textures/{skinHash}"
                : $"{domain}/api/skin/{Uri.EscapeDataString(username)}.png?v={timestamp}";

            var skinMeta = new Dictionary<string, object>
            {
                ["url"] = skinUrl
            };

            if (string.Equals(model, "slim", StringComparison.OrdinalIgnoreCase))
            {
                skinMeta["metadata"] = new { model = "slim" };
            }

            textures["SKIN"] = skinMeta;

            if (_skinStorage.HasCape(username))
            {
                var capeHash = _skinStorage.GetCapeHash(username);
                var capeUrl = !string.IsNullOrEmpty(capeHash)
                    ? $"{domain}/textures/{capeHash}"
                    : $"{domain}/api/cape/{Uri.EscapeDataString(username)}.png?v={timestamp}";

                textures["CAPE"] = new
                {
                    url = capeUrl
                };
            }

            var cleanId = userId.Replace("-", "");
            var payloadObj = new
            {
                timestamp = timestamp,
                profileId = cleanId,
                profileName = username,
                signatureRequired = true,
                textures = textures
            };

            var payloadJson = JsonSerializer.Serialize(payloadObj);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));

            var signatureBytes = _rsa.SignData(Encoding.UTF8.GetBytes(payloadBase64), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            var signatureBase64 = Convert.ToBase64String(signatureBytes);

            return new PropertyDto("textures", payloadBase64, signatureBase64);
        }

        public static string GenerateOfflineUuid(string username)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes("OfflinePlayer:" + username));
            hash[6] = (byte)((hash[6] & 0x0f) | 0x30);
            hash[8] = (byte)((hash[8] & 0x3f) | 0x80);

            var sb = new StringBuilder(32);
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
