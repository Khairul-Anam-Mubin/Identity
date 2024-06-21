using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForPasswordGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenService tokenService) : ICommandHandler<CreateTokenForPasswordGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    
    public async Task<IResult<TokenResponse>> Handle(
        CreateTokenForPasswordGrantTypeCommand command, 
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return OAuthError.InvalidClient.InResult<TokenResponse>();
        }

        var user = await _userRepository.GetUserByUserNameAsync(command.UserName);

        if (user is null)
        {
            return Result.Failure<TokenResponse>(OAuthError.NoAccess);
        }

        if (!user.Password.IsMatch(command.Password))
        {
            return Result.Failure<TokenResponse>(OAuthError.NoAccess);
        }

        var refreshTokenCreateResult = await _tokenService.CreateUserRefreshTokenAsync(user, client);

        if (refreshTokenCreateResult.IsFailure || refreshTokenCreateResult.Value is null)
        {
            return Result.Create<TokenResponse>(refreshTokenCreateResult);
        }

        var accessTokenCreateResult = await _tokenService.CreateUserAccessTokenAsync(user, client);

        if (accessTokenCreateResult.IsFailure || accessTokenCreateResult.Value is null)
        {
            return Result.Create<TokenResponse>(accessTokenCreateResult);
        }

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessTokenCreateResult.Value, 
            _tokenService.AccessTokenExpirationTimeInSecond(), 
            refreshTokenCreateResult.Value, 
            null);

        return Result.Success(tokenResponse);
    }
}
