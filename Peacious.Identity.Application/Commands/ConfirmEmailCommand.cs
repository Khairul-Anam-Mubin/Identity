using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record ConfirmEmailCommand(
    string Email, 
    string VerifierCode) : ICommand;