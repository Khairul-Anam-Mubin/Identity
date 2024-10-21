using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public class TokenRequestToRefreshTokenGrantTypeCommandConverter
    : TokenRequestToCommandConverter
{
    protected override bool CanConvert(TokenRequest tokenRequest)
    {
        return tokenRequest.GrantType == GrantType.RefreshToken;
    }

    protected override ICommand<TokenResponse>? Convert(TokenRequest tokenRequest)
    {
        return new CreateTokenForRefreshTokenGrantTypeCommand(
                    tokenRequest.ClientId,
                    tokenRequest.RefreshToken!);
    }
}
