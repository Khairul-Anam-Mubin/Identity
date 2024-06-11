using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record RemovePermissionsFromRoleCommand(
    string RoleId,
    List<string> PermissionIds) : ICommand;
