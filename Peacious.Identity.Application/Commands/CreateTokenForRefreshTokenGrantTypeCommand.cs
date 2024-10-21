using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForRefreshTokenGrantTypeCommand(
    [Required] string ClientId,
    [Required] string RefreshToken) : ICommand<TokenResponse>;
