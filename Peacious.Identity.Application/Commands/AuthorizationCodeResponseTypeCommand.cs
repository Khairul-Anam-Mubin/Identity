using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Application.Commands;

public record AuthorizationCodeResponseTypeCommand(
    string ClientId,
    string RedirectUri,
    string UserId,
    string? Scope,
    string? State,
    string? CodeChallange,
    string? CodeChallangeMethod) : ICommand<AuthorizationResponse>;