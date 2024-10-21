using Peacious.Framework.Results;
using Peacious.Identity.Domain.Entities;
using System.Security.Claims;

namespace Peacious.Identity.Application.Services;

public interface ITokenService
{
    IResult ValidateToken(string jwtToken,
        bool validateIssuer = true,
        bool validateAudience = true,
        bool validateLifetime = true,
        bool validateIssuerSigningKey = true);

    public int AccessTokenExpirationTimeInSecond();

    public int RefreshTokenExpirationTimeInSecond();

    Task<string> CreateClientAccessTokenAsync(User user, Client client);

    string CreateUserAccessToken(User user, Client client, string? scope);

    Task<string> CreateUserRefreshTokenAsync(TokenSession refreshTokenSession, List<Claim> refreshTokenClaims);

    string GenerateRefreshToken(List<Claim> claims);
    string GenerateAccessToken(List<Claim> claims);
}