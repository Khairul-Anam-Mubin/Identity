using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public class TokenRequestToAuthorizationCodeGrantTypeCommandConverter
    : TokenRequestToCommandConverter
{
    protected override bool CanConvert(TokenRequest tokenRequest)
    {
        return tokenRequest.GrantType == GrantType.AuthorizationCode;
    }

    protected override ICommand<TokenResponse>? Convert(TokenRequest tokenRequest)
    {
        return new CreateTokenForAuthorizationCodeGrantTypeCommand(
                    tokenRequest.ClientId,
                    tokenRequest.Code!,
                    tokenRequest.RedirectUri!,
                    tokenRequest.CodeVerifier,
                    tokenRequest.ClientSecret);
    }
}
