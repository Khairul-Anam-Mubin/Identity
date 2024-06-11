using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record ChangePasswordRequest
{
    [JsonPropertyName("old_password")]
    public required string OldPassword { get; set; }

    [JsonPropertyName("new_password")]
    public required string NewPassword { get; set; }
}
