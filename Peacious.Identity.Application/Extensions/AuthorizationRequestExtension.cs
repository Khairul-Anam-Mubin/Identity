using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Application.Extensions;

public static class AuthorizationRequestExtension
{
    public static ICommand<AuthorizationResponse>? ToAuthorizationResponseTypeCommand(this AuthorizationRequest request, string userId)
    {
        return request.ResponseType switch
        {
            ResponseType.Code => 
                new AuthorizationCodeResponseTypeCommand(
                    request.ClientId,
                    request.RedirectUri,
                    userId,
                    request.Scope,
                    request.State,
                    request.CodeChallenge,
                    request.CodeChallengeMethod),
            ResponseType.Token => null,
            _ => null,
        };
    }
}
