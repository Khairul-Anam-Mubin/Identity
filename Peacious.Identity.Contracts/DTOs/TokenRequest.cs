using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record TokenRequest
{
    [JsonPropertyName("grant_type")]
    public required string GrantType { get; set; }

    [JsonPropertyName("client_id")]
    public required string ClientId { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }
    public string? Password { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }

    public string? Code { get; set; }

    [JsonPropertyName("redirect_uri")]
    public string? RedirectUri { get; set; }

    [JsonPropertyName("code_verifier")]
    public string? CodeVerifier { get; set; }
}