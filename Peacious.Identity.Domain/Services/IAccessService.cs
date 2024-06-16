using Peacious.Framework.Results;
using Peacious.Identity.Domain.Entities;
using System.Security.Claims;

namespace Peacious.Identity.Domain.Services;

public interface IAccessService
{
    public int AccessTokenExpirationTimeInSecond();

    public int RefreshTokenExpirationTimeInSecond();

    string CreateUserAccessToken(
        User user, Client client, List<Role> roles, List<Permission> permissions);

    Task<string> CreateUserRefreshTokenAsync(User user, Client client, List<Role> roles, List<Permission> permissions);

    List<Claim> ConvertToClaims(
        User user, Client client, List<Role> roles, List<Permission> permissions);

    Task<IResult<string>> CreateUserAccessTokenAsync(string userId, string clientId);

    Task<IResult<string>> CreateUserRefreshTokenAsync(string userId, string clientId);
}