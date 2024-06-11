using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record ChangeNameRequest
{
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }
}