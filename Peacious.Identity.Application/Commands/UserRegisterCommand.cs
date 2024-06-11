using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record UserRegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand;