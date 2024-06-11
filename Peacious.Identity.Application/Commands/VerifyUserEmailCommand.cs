using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record VerifyUserEmailCommand(
    string Email, 
    string VerifierCode) : ICommand;