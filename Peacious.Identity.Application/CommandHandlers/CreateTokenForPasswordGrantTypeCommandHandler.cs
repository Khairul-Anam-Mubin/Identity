using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Repositories;
using Peacious.Identity.Domain.Services;

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
            return Result.Error<TokenResponse>("Invalid Client Id");
        }

        var user = await _userRepository.GetUserByUserNameAsync(command.UserName);

        if (user is null)
        {
            return Result.Error<TokenResponse>("User not found.");
        }

        if (!user.Password.IsMatch(command.Password))
        {
            return Result.Error<TokenResponse>("Password Error.");
        }

        var refreshTokenCreateResult = await _tokenService.CreateUserRefreshTokenAsync(user, client);

        if (refreshTokenCreateResult.IsFailure() || refreshTokenCreateResult.Value is null)
        {
            return Result.Error<TokenResponse>(refreshTokenCreateResult.Message);
        }

        var accessTokenCreateResult = await _tokenService.CreateUserAccessTokenAsync(user, client);

        if (accessTokenCreateResult.IsFailure() || accessTokenCreateResult.Value is null)
        {
            return Result.Error<TokenResponse>(accessTokenCreateResult.Message);
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
