using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record CreateClientCredentialCommand(
    string ClientName,
    string ClientWebsite,
    string? LogoUrl,
    string RedirectUri,
    List<string> PermissionIds) : ICommand;