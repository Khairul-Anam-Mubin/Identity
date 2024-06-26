﻿using Peacious.Framework.CQRS;

namespace Peacious.Identity.Application.Commands;

public record AuthorizationTokenResponseTypeCommand(
    string ClientId,
    string RedirectUri,
    string? Scope,
    string? State,
    string? CodeChallenge,
    string? CodeChallengeMethod) : ICommand;
