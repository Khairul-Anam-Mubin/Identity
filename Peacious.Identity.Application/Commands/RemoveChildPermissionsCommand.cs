using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record RemoveChildPermissionsCommand(
    string PermissionId,
    List<string> ChildPermissionIds) : ICommand;
