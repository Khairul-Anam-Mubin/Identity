using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenCommand(
    string GrantType,
    string ClientId,
    string? UserName,
    string? Password,
    string? RefreshToken,
    string? ClientSecret,
    string? Code,
    string? RedirectUri,
    string? CodeVerifier) : ICommand;