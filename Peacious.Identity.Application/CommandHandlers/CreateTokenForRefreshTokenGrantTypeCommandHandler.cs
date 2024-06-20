using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Repositories;
using Peacious.Identity.Domain.Services;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForRefreshTokenGrantTypeCommandHandler(
    IClientRepository clientRepository,
    ITokenSessionRepository tokenSessionRepository,
    ITokenService tokenService)
    : ICommandHandler<CreateTokenForRefreshTokenGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;

    public async Task<IResult<TokenResponse>> Handle(CreateTokenForRefreshTokenGrantTypeCommand command, CancellationToken cancellationToken)
    {
        var tokenValidationResult = _tokenService.ValidateToken(command.RefreshToken);

        if (tokenValidationResult.IsFailure())
        {
            return Result.Error<TokenResponse>(tokenValidationResult.Message);
        }

        var claims = TokenHelper.GetClaims(command.RefreshToken);

        var tokenSessionId = claims.FirstOrDefault(claim => claim.Type == "jti")?.Value;

        if (string.IsNullOrEmpty(tokenSessionId))
        {
            return Result.Error<TokenResponse>("jti not found after parsing the token");
        }

        var clientId = claims.FirstOrDefault(claim => claim.Type == "client_id")?.Value;

        if (string.IsNullOrEmpty(clientId))
        {
            return Result.Error<TokenResponse>("client_id not found after parsing the token");
        }

        if (!clientId.Equals(command.ClientId))
        {
            return Result.Error<TokenResponse>(
                "Token refresh is not permissible from another client. Provide the exact valid client_id");
        }

        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is null)
        {
            return Result.Error<TokenResponse>("Invalid Client Id");
        }

        var tokenSession = await _tokenSessionRepository.GetByIdAsync(tokenSessionId);

        if (tokenSession is null)
        {
            return Result.Error<TokenResponse>("TokenSession not found");
        }

        var tokenSessionRefreshResult = tokenSession.Refresh();

        if (tokenSessionRefreshResult.IsFailure())
        {
            return Result.Create<TokenResponse>(tokenSessionRefreshResult);
        }

        await _tokenSessionRepository.SaveAsync(tokenSession);

        var refreshToken = await _tokenService.CreateUserRefreshTokenAsync(tokenSession, claims);

        var accessToken = _tokenService.CreateUserAccessToken(tokenSession, claims);

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken,
            _tokenService.AccessTokenExpirationTimeInSecond(),
            refreshToken,
            tokenSession.Scope);

        return Result.Success(tokenResponse);
    }
}
