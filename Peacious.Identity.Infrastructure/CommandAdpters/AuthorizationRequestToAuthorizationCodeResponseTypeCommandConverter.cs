using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public class AuthorizationRequestToAuthorizationCodeResponseTypeCommandConverter
    : AuthorizationRequestToCommandConverter
{
    protected override bool CanConvert(AuthorizationRequest request)
    {
        return request.ResponseType == ResponseType.Code;
    }
    protected override ICommand<AuthorizationResponse>? Convert(AuthorizationRequest request)
    {
        return new AuthorizationCodeResponseTypeCommand(
                    request.ClientId,
                    request.RedirectUri,
                    request.Scope,
                    request.State,
                    request.CodeChallenge,
                    request.CodeChallengeMethod);
    }
}
