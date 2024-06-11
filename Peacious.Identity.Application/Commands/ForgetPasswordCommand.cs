using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record ForgetPasswordCommand(string UserName) : ICommand;
