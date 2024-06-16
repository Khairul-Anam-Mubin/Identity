﻿using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.Models;
using System.ComponentModel.DataAnnotations;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForClientCredentialsGrantTypeCommand(
    [Required] string ClientId,
    [Required] string ClientSecret) : ICommand<TokenResponse>;