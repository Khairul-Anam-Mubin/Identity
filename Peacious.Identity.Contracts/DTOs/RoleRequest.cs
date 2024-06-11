using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record RoleRequest
{
    [JsonPropertyName("role_name")]
    public required string RoleName { get; set; }
}
