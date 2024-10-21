using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record UserRegisterCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password) : ICommand;