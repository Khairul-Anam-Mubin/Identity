using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record ForgetPasswordRequest
{
    [JsonPropertyName("username")]
    public required string UserName { get; set; }
}
