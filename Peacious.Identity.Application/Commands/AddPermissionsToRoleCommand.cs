using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AddPermissionsToRoleCommand(
    string RoleId, 
    List<string> PermissionIds) : ICommand;
