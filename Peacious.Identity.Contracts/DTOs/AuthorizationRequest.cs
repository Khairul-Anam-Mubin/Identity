using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record AuthorizationRequest 
{
    [JsonPropertyName("response_type")]
    public required string ResponseType { get; set; }

    [JsonPropertyName("client_id")]
    public required string ClientId { get; set; }

    [JsonPropertyName("redirect_uri")]
    public required string RedirectUri { get; set; }

    public string? Scope { get; set; }
    public string? State { get; set; }

    [JsonPropertyName("code_challange")]
    public string? CodeChallange { get; set; }

    [JsonPropertyName("code_challange_method")]
    public string? CodeChallangeMethod { get; set; }
}