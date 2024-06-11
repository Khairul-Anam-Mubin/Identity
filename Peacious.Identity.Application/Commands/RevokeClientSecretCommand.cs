using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record RevokeClientSecretCommand(
    string ClientId) : ICommand;
