using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AuthorizationCodeResponseTypeCommand(
    string ClientId,
    string RedirectUri,
    string? Scope,
    string? State,
    string? CodeChallange,
    string? CodeChallangeMethod) : ICommand;