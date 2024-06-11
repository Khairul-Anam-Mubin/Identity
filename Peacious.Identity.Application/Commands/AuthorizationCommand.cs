using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AuthorizationCommand(
    string ResponseType,
    string ClientId,
    string? RedirectUri,
    string? Scope,
    string? State,
    string? CodeChallange,
    string? CodeChallangeMethod) : ICommand;
