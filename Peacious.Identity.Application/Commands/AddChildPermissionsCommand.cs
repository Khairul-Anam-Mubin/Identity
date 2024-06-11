using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AddChildPermissionsCommand(
    string PermissionId,
    List<string> ChildPermissionIds) : ICommand;
