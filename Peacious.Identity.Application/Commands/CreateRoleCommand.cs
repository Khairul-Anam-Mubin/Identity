using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record CreateRoleCommand(string RoleName) : ICommand;
