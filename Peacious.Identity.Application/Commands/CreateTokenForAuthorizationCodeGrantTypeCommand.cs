using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.Models;
using System.ComponentModel.DataAnnotations;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForAuthorizationCodeGrantTypeCommand(
    [Required] string ClientId,
    [Required] string Code,
    [Required] string RedirectUri,
    string? CodeVerifier,
    string? ClientSecret) : ICommand<TokenResponse>;
