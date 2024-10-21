using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.DTOs;

public record UserRegistrationRequest
{
    [JsonPropertyName("first_name")]
    public required string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; set; }
    [JsonPropertyName("username")]
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
