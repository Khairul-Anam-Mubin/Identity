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
    IAccessService accessService) : ICommandHandler<CreateTokenForPasswordGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAccessService _accessService = accessService;
    
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

        var accessTokenResult = await _accessService.CreateUserAccessTokenAsync(user.Id, client.Id);

        if (accessTokenResult.IsFailure() || accessTokenResult.Value is null)
        {
            return Result.Error<TokenResponse>(accessTokenResult.Message);
        }

        var refreshTokenResult = await _accessService.CreateUserAccessTokenAsync(user.Id, client.Id);

        if (refreshTokenResult.IsFailure() || refreshTokenResult.Value is null)
        {
            return Result.Error<TokenResponse>(refreshTokenResult.Message);
        }

        var tokenResponse = TokenResponse.Create(
            TokenType.Bearer,
            accessTokenResult.Value, 
            _accessService.AccessTokenExpirationTimeInSecond(), 
            refreshTokenResult.Value, 
            null);

        return Result.Success(tokenResponse);
    }
}
