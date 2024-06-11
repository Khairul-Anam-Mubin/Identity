using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record GenerateNewClientSecretCommand(
    string ClientId) : ICommand;
