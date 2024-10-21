using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Extensions;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;
using System.Security.Claims;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForClientCredentialsGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenService tokenService) 
    : ICommandHandler<CreateTokenForClientCredentialsGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<IResult<TokenResponse>> Handle(CreateTokenForClientCredentialsGrantTypeCommand command, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return OAuthError.InvalidClient(command.ClientId).Result<TokenResponse>();
        }

        if (!client.Secret.IsMatch(command.ClientSecret))
        {
            return OAuthError.InvalidCredentials.Result<TokenResponse>();
        }
        
        var user = await _userRepository.GetUserByUserNameAsync(client.UserName);

        if (user is null)
        {
            return OAuthError.InvalidUser(client.UserName).Result<TokenResponse>();
        }

        var role = string.Empty;

        var accessTokenClaims = new List<Claim>
        {
            new Claim(ClaimType.JwtTokenId, Guid.NewGuid().ToString()),
            new Claim(ClaimType.Role, role),
            new Claim(ClaimType.Source, GrantType.ClientCredentials)
        };
        
        var userClaims = user.ToClaims();
        var clientClaims = client.ToClaims();
        
        accessTokenClaims.AddRange(clientClaims);
        accessTokenClaims.AddRange(userClaims);

        var accessToken = _tokenService.GenerateAccessToken(accessTokenClaims);

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken,
            _tokenService.AccessTokenExpirationTimeInSecond(),
            null,
            null);

        return Result.Success(tokenResponse);
    }
}
