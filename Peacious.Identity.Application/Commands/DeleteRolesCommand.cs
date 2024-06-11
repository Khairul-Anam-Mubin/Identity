using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record DeleteRolesCommand(List<string> RoleIds): ICommand;
