using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Contracts.Models;

namespace Peacious.Identity.Application.Extensions;

public static class TokenRequestExtension
{
    public static ICommand<TokenResponse>? ToCreateTokenByGrantTypeCommand(this TokenRequest request)
    {
        return request.GrantType switch
        {
            GrantType.Password => 
                new CreateTokenForPasswordGrantTypeCommand(
                    request.ClientId,
                    request.UserName!,
                    request.Password!),
            GrantType.RefreshToken => 
                new CreateTokenForRefreshTokenGrantTypeCommand(
                    request.ClientId,
                    request.RefreshToken!),
            GrantType.AuthorizationCode => 
                new CreateTokenForAuthorizationCodeGrantTypeCommand(
                    request.ClientId,
                    request.Code!,
                    request.RedirectUri!,
                    request.CodeVerifier!,
                    request.ClientSecret!),
            GrantType.ClientCredentials => 
                new CreateTokenForClientCredentialsGrantTypeCommand(
                    request.ClientId,
                    request.ClientSecret!),
            _ => default,
        };
    }
}
