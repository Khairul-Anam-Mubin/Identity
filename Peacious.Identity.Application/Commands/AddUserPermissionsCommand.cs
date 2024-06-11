using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AddUserPermissionsCommand(
    string UserId, 
    List<string> PermissionIds) : ICommand;
