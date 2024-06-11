using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record RemoveUserPermissionsCommand(
    string UserId, 
    List<string> PermissionIds) : ICommand;
