using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record ChangePasswordCommand(
    string UserId,
    string OldPassword,
    string NewPassword) : ICommand;
