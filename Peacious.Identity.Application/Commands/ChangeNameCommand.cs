using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record ChangeNameCommand(
    string? FirstName,
    string? LastName,
    string? UserName) : ICommand;