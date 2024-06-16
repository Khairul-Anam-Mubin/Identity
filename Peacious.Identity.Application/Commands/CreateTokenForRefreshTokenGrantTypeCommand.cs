using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.Models;
using System.ComponentModel.DataAnnotations;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForRefreshTokenGrantTypeCommand(
    [Required] string ClientId,
    [Required] string RefreshToken) : ICommand<TokenResponse>;
