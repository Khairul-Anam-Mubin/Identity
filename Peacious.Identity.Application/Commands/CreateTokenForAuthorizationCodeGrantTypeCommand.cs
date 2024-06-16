using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.Models;
using System.ComponentModel.DataAnnotations;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForAuthorizationCodeGrantTypeCommand(
    [Required] string ClientId,
    [Required] string Code,
    [Required] string RedirectUri,
    [Required] string CodeVerifier,
    [Required] string ClientSecret) : ICommand<TokenResponse>;
