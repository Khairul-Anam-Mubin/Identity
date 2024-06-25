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

    [JsonPropertyName("code_challenge")]
    public string? CodeChallenge { get; set; }

    [JsonPropertyName("code_challenge_method")]
    public string? CodeChallengeMethod { get; set; }
}