using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record OAuth2ErrorResponse
{
    public required string Error { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("error_description")]
    public string? Description { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("error_uri")]
    public string? Uri { get; set; }
}
