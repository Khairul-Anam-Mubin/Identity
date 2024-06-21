using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;

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
            return Result.Failure<TokenResponse>(OAuthError.InvalidClient);
        }

        if (!client.Secret.IsMatch(command.ClientSecret))
        {
            return Result.Failure<TokenResponse>(OAuthError.NoAccess);
        }
        
        var user = await _userRepository.GetUserByUserNameAsync(client.UserName);

        if (user is null)
        {
            return Result.Failure<TokenResponse>(OAuthError.NoAccess);
        }

        var accessToken = await _tokenService.CreateClientAccessTokenAsync(user, client);

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken,
            _tokenService.AccessTokenExpirationTimeInSecond(),
            null,
            null);

        return Result.Success(tokenResponse);
    }
}
