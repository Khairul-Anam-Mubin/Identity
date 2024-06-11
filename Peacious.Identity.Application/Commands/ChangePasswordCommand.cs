using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record ChangePasswordCommand(
    string OldPassword,
    string NewPassword) : ICommand;
