using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record DeletePermissionsCommand(
    List<string> PermissionIds) : ICommand;
