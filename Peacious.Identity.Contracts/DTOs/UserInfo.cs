namespace Peacious.Identity.Contracts.DTOs;

public record UserInfo(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Email);
