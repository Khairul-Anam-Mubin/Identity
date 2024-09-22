using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Contracts.Models;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public class TokenRequestToPasswordGrantTypeCommandConverter 
    : TokenRequestToCommandConverter
{
    protected override bool CanConvert(TokenRequest tokenRequest)
    {
        return tokenRequest.GrantType == GrantType.Password;
    }

    protected override ICommand<TokenResponse>? Convert(TokenRequest tokenRequest)
    {
        return new CreateTokenForPasswordGrantTypeCommand(
                    tokenRequest.ClientId,
                    tokenRequest.UserName!,
                    tokenRequest.Password!);
    }
}
