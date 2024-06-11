using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AddUserRolesCommand(
    string UserId, 
    List<string> RoleIds) : ICommand;
