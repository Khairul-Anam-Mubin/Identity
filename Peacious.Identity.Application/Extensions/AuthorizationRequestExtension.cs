using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Application.Extensions
{
    public static class AuthorizationRequestExtension
    {
        public static ICommand? ToAuthorizationResponseTypeCommand(this AuthorizationRequest request)
        {
            return request.ResponseType switch
            {
                ResponseType.Code => 
                    new AuthorizationCodeResponseTypeCommand(
                        request.ClientId,
                        request.RedirectUri,
                        request.Scope,
                        request.State,
                        request.CodeChallange,
                        request.CodeChallangeMethod),
                ResponseType.Token => 
                    new AuthorizationCodeResponseTypeCommand(
                        request.ClientId,
                        request.RedirectUri,
                        request.Scope,
                        request.State,
                        request.CodeChallange,
                        request.CodeChallangeMethod),
                _ => null,
            };
        }
    }
}
