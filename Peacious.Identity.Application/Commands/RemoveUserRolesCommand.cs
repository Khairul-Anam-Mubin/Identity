using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record RemoveUserRolesCommand(
    string UserId, 
    List<string> RoleIds) : ICommand;
