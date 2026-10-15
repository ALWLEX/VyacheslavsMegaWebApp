using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VyacheslavsMegaWebApp.Api.Models
{
    public sealed record ProfileDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name
    );

    public sealed record UserDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("username")] string Username,
        [property: JsonPropertyName("minecraftUsername")] string? MinecraftUsername = null,
        [property: JsonPropertyName("skinModel")] string SkinModel = "classic",
        [property: JsonPropertyName("skinUrl")] string? SkinUrl = null,
        [property: JsonPropertyName("capeUrl")] string? CapeUrl = null
    );

    public sealed record PropertyDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("value")] string Value,
        [property: JsonPropertyName("signature")] string? Signature = null
    );

    public sealed record AuthenticateRequest(
        [property: JsonPropertyName("username")] string? Username,
        [property: JsonPropertyName("password")] string? Password,
        [property: JsonPropertyName("clientToken")] string? ClientToken = null,
        [property: JsonPropertyName("requestUser")] bool RequestUser = true
    );

    public sealed record AuthResponse(
        [property: JsonPropertyName("accessToken")] string AccessToken,
        [property: JsonPropertyName("clientToken")] string ClientToken,
        [property: JsonPropertyName("selectedProfile")] ProfileDto SelectedProfile,
        [property: JsonPropertyName("availableProfiles")] List<ProfileDto> AvailableProfiles,
        [property: JsonPropertyName("user")] UserDto User,
        [property: JsonPropertyName("skinUrl")] string? SkinUrl = null
    );

    public sealed record ValidateRequest(
        [property: JsonPropertyName("accessToken")] string? AccessToken,
        [property: JsonPropertyName("clientToken")] string? ClientToken = null
    );

    public sealed record RefreshRequest(
        [property: JsonPropertyName("accessToken")] string? AccessToken,
        [property: JsonPropertyName("clientToken")] string? ClientToken = null,
        [property: JsonPropertyName("requestUser")] bool RequestUser = true
    );

    public sealed record InvalidateRequest(
        [property: JsonPropertyName("accessToken")] string? AccessToken,
        [property: JsonPropertyName("clientToken")] string? ClientToken = null
    );

    public sealed record JoinServerRequest(
        [property: JsonPropertyName("accessToken")] string AccessToken,
        [property: JsonPropertyName("selectedProfile")] string SelectedProfile,
        [property: JsonPropertyName("serverId")] string ServerId
    );

    public sealed record HasJoinedResponse(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("properties")] List<PropertyDto> Properties
    );

    public sealed record YggdrasilMetaResponse(
        [property: JsonPropertyName("meta")] object Meta,
        [property: JsonPropertyName("skinDomains")] string[] SkinDomains,
        [property: JsonPropertyName("signaturePublickey")] string SignaturePublickey
    );
}
