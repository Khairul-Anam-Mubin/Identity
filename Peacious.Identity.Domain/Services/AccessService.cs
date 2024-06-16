using Peacious.Framework.Identity;
using Peacious.Framework.Results;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;
using System.Security.Claims;

namespace Peacious.Identity.Domain.Services;

public class AccessService(
    TokenConfig tokenConfig,
    ITokenSessionRepository tokenSessionRepository,
    IUserRepository userRepository,
    IClientRepository clientRepository,
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository) : IAccessService
{
    private readonly TokenConfig _tokenConfig = tokenConfig;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;
    
    public List<Claim> ConvertToClaims(
        User user, Client client, List<Role> roles, List<Permission> permissions)
    {
        var scopeClaims =
            permissions.Select(permission => permission.ToClaim());

        var roleClaims = roles.Select(role => role.ToClaim());

        var userClaims = user.ToClaims();

        var clientClaims = client.ToClaims();

        var allClaims = new List<Claim>();

        allClaims.AddRange(clientClaims);
        allClaims.AddRange(userClaims);
        allClaims.AddRange(roleClaims);
        allClaims.AddRange(scopeClaims);

        return userClaims;
    }

    public string CreateUserAccessToken(User user, Client client, List<Role> roles, List<Permission> permissions)
    {
        var userClaims = ConvertToClaims(user, client, roles, permissions);

        userClaims.Add(new Claim("jti", Guid.NewGuid().ToString()));

        var accessToken = TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.ExpirationTimeInSec,
            userClaims);

        return accessToken;
    }

    public async Task<string> CreateUserRefreshTokenAsync(User user, Client client, List<Role> roles, List<Permission> permissions)
    {
        var tokenSession = TokenSession.Create(
            user.Id,
            client.Id,
            permissions,
            roles,
            DateTime.UtcNow.AddSeconds(_tokenConfig.RefreshTokenExpirationTimeInSec));

        await _tokenSessionRepository.SaveAsync(tokenSession);

        var userClaims = user.ToClaims();
        var clientClaims = client.ToClaims();

        var refreshTokenClaims = new List<Claim>
        {
            new Claim("jti", tokenSession.Id)
        };

        refreshTokenClaims.AddRange(clientClaims);
        refreshTokenClaims.AddRange(userClaims);

        return TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.RefreshTokenExpirationTimeInSec,
            refreshTokenClaims);
    }
    
    public async Task<IResult<string>> CreateUserAccessTokenAsync(string userId, string clientId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null) return Result.Error<string>("User not found while creating access token.");
        
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is null) return Result.Error<string>("Client not found while creating access token.");
        
        var roles = await _roleRepository.GetUserRolesAsync(userId);
        var permissions = await _permissionRepository.GetUserPermissionsAsync(userId);

        var accessToken = CreateUserAccessToken(user, client, roles, permissions);

        return Result.Success(accessToken, "");
    }

    public async Task<IResult<string>> CreateUserRefreshTokenAsync(string userId, string clientId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null) return Result.Error<string>("User not found while creating refresh token.");

        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is null) return Result.Error<string>("Client not found while creating refresh token.");

        var roles = await _roleRepository.GetUserRolesAsync(userId);
        var permissions = await _permissionRepository.GetUserPermissionsAsync(userId);

        var refreshToken = await CreateUserRefreshTokenAsync(user, client, roles, permissions);

        return Result.Success(refreshToken, "");
    }

    public int AccessTokenExpirationTimeInSecond() => _tokenConfig.ExpirationTimeInSec;
    public int RefreshTokenExpirationTimeInSecond() => _tokenConfig.RefreshTokenExpirationTimeInSec;
}
