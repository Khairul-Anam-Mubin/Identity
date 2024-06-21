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

    string CreateUserAccessToken(
        User user, Client client, List<Role> roles, List<Permission> permissions);

    Task<IResult<string>> CreateUserAccessTokenAsync(User user, Client client);

    Task<IResult<string>> CreateUserAccessTokenAsync(string userId, string clientId);

    string CreateUserAccessToken(TokenSession refreshTokenSession, List<Claim> refreshTokenClaims);

    Task<string> CreateUserRefreshTokenAsync(User user, Client client, List<Role> roles, List<Permission> permissions);

    Task<IResult<string>> CreateUserRefreshTokenAsync(User user, Client client);

    Task<IResult<string>> CreateUserRefreshTokenAsync(string userId, string clientId);

    Task<string> CreateUserRefreshTokenAsync(TokenSession refreshTokenSession, List<Claim> refreshTokenClaims);
}