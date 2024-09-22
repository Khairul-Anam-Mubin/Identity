using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Contracts.Models;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public class TokenRequestToClientCredentialsGrantTypeCommandConverter 
    : TokenRequestToCommandConverter
{
    protected override bool CanConvert(TokenRequest tokenRequest)
    {
        return tokenRequest.GrantType == GrantType.ClientCredentials;
    }

    protected override ICommand<TokenResponse>? Convert(TokenRequest tokenRequest)
    {
        return new CreateTokenForClientCredentialsGrantTypeCommand(
                tokenRequest.ClientId,
                tokenRequest.ClientSecret!);
    }
}
