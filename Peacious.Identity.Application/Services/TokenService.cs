using Microsoft.IdentityModel.Tokens;
using Peacious.Framework.IdentityScope;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Extensions;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;
using System.Security.Claims;
using System.Text;

namespace Peacious.Identity.Application.Services;

public class TokenService(
    TokenConfig tokenConfig,
    ITokenSessionRepository tokenSessionRepository,
    IUserRepository userRepository,
    IClientRepository clientRepository,
    IPermissionRepository permissionRepository) : ITokenService
{
    private readonly TokenConfig _tokenConfig = tokenConfig;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;

    public IResult ValidateToken(string jwtToken,
        bool validateIssuer = true,
        bool validateAudience = true,
        bool validateLifetime = true,
        bool validateIssuerSigningKey = true)
    {
        var validationParameter = GetTokenValidationParameters(
            validateIssuer,
            validateAudience,
            validateLifetime,
            validateIssuerSigningKey);

        if (TokenHelper.TryValidateToken(jwtToken, validationParameter, out string validationMessage))
        {
            return Result.Success(validationMessage);
        }

        return OAuthError.InvalidToken(validationMessage).Result();
    }

    public async Task<string> CreateClientAccessTokenAsync(User user, Client client)
    {
        var permissions = await _permissionRepository.GetClientPermissionsAsync(client.Id);

        var userClaims = user.ToClaims();
        var clientClaims = client.ToClaims();
        var scopeClaims =
            permissions.Select(permission => permission.ToClaim());

        var allClaims = new List<Claim>
        {
            new Claim(ClaimType.JwtTokenId, Guid.NewGuid().ToString())
        };

        allClaims.AddRange(userClaims);
        allClaims.AddRange(clientClaims);
        allClaims.AddRange(scopeClaims);

        return GenerateAccessToken(allClaims);
    }

    public string GenerateAccessToken(List<Claim> claims)
    {
        var token = TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.ExpirationTimeInSec,
            claims);

        return token;
    }

    public string CreateUserAccessToken(User user, Client client, string? scope)
    {
        var claims = new List<Claim>();

        var userClaims = user.ToClaims();
        var clientClaims = client.ToClaims();

        claims.AddRange(userClaims);
        claims.AddRange(clientClaims);
        claims.Add(new Claim(ClaimType.JwtTokenId, Guid.NewGuid().ToString()));

        if (!string.IsNullOrEmpty(scope))
        {
            var scopes = scope?.Split(' ')?.ToList() ?? new List<string>();

            scopes.ForEach(scope => claims.Add(new Claim(ClaimType.Scope, scope)));
        }

        return GenerateAccessToken(claims);
    }

    public string GenerateRefreshToken(List<Claim> claims)
    {
        var token = TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.RefreshTokenExpirationTimeInSec,
            claims);

        return token;
    }

    public async Task<string> CreateUserRefreshTokenAsync(TokenSession refreshTokenSession, List<Claim> refreshTokenClaims)
    {
        var newTokenSession =
            TokenSession.Create(
                refreshTokenSession.UserId,
                refreshTokenSession.ClientId,
                refreshTokenSession.Scope,
                DateTime.UtcNow.AddSeconds(_tokenConfig.RefreshTokenExpirationTimeInSec),
                refreshTokenSession.GrantType);

        await _tokenSessionRepository.SaveAsync(newTokenSession);

        var claims = GetDeterminedClaims(refreshTokenClaims);

        claims.Add(new Claim(ClaimType.JwtTokenId, newTokenSession.Id));

        return GenerateRefreshToken(claims);
    }

    public int AccessTokenExpirationTimeInSecond() => _tokenConfig.ExpirationTimeInSec;
    public int RefreshTokenExpirationTimeInSecond() => _tokenConfig.RefreshTokenExpirationTimeInSec;

    private List<Claim> GetDeterminedClaims(List<Claim> claims)
    {
        return claims.Where(x =>
               x.Type != ClaimType.JwtTokenId &&
               x.Type != ClaimType.Audience &&
               x.Type != ClaimType.Issuer &&
               x.Type != ClaimType.ExpirationTime)
           .ToList();
    }

    private TokenValidationParameters GetTokenValidationParameters(
        bool validateIssuer = true,
        bool validateAudience = true,
        bool validateLifetime = true,
        bool validateIssuerSigningKey = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = validateIssuer,
            ValidateAudience = validateAudience,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = validateIssuerSigningKey,

            ClockSkew = TimeSpan.Zero,
            ValidIssuer = _tokenConfig.Issuer,
            ValidAudience = _tokenConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_tokenConfig.SecretKey))
        };
    }
}
