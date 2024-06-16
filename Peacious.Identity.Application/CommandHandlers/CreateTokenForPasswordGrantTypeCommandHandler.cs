using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;
using System.Security.Claims;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForPasswordGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    IPermissionRepository permissionRepository,
    IRoleRepository roleRepository,
    ITokenSessionRepository tokenSessionRepository,
    TokenConfig tokenConfig) : ICommandHandler<CreateTokenForPasswordGrantTypeCommand>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;
    private readonly TokenConfig _tokenConfig = tokenConfig;
    
    public async Task<IResult> Handle(
        CreateTokenForPasswordGrantTypeCommand command, 
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return Result.Error("Invalid Client Id");
        }

        var user = await _userRepository.GetUserByUserNameAsync(command.UserName);

        if (user is null)
        {
            return Result.Error("User not found.");
        }

        if (!user.Password.IsMatch(command.Password))
        {
            return Result.Error("Password Error.");
        }

        var permissions = await _permissionRepository.GetUserPermissionsAsync(user.Id);

        var roles = await _roleRepository.GetUserRolesAsync(user.Id);

        var scopeClaims = 
            permissions.Select(permission => new Claim("scope", permission.Title));

        var roleClaims = roles.Select(role => new Claim("role", role.Name));

        var userIdClaim = new Claim("user_id", user.Id);

        var userNameClaim = new Claim("username", user.UserName);

        var clientIdClaim = new Claim("client_id", client.Id);

        var userClaims = new List<Claim>
        {
            userIdClaim,
            userNameClaim,
            clientIdClaim,
        };

        userClaims.AddRange(roleClaims);
        userClaims.AddRange(scopeClaims);

        var accessToken = TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.ExpirationTimeInSec,
            userClaims);

        var tokenSession = TokenSession.Create(
            user.Id,
            client.Id,
            permissions,
            roles,
            DateTime.UtcNow.AddSeconds(_tokenConfig.RefreshTokenExpirationTimeInSec));

        await _tokenSessionRepository.SaveAsync(tokenSession);

        var refreshTokenClaims = new List<Claim>
        {
            userIdClaim,
            userNameClaim,
            clientIdClaim,
            new Claim("token_id", tokenSession.Id)
        };

        var refreshToken = TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.RefreshTokenExpirationTimeInSec,
            refreshTokenClaims);

        var tokenResponse = Token.Create(
            "bearer", 
            accessToken, 
            _tokenConfig.ExpirationTimeInSec, 
            refreshToken, 
            tokenSession.Scope);

        var result = Result.Success();
        
        result.SetData("TokenResponse", tokenResponse);

        return result;
    }
}
