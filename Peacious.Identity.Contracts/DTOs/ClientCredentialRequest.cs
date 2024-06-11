using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record ClientCredentialRequest
{
    [JsonPropertyName("client_name")]
    public required string ClientName { get; set; }

    [JsonPropertyName("client_website")]
    public required string ClientWebsite { get; set; }

    [JsonPropertyName("logo_url")]
    public string? LogoUrl { get; set; }

    [JsonPropertyName("redirect_uri")]
    public required string RedirectUri { get; set; }

    [JsonPropertyName("permission_ids")]
    public List<string> PermissionIds { get; set; }

    public ClientCredentialRequest()
    {
        PermissionIds = new List<string>();
    }
}