using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record CreatePermissionCommand(string Title) : ICommand;