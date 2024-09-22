using Peacious.Framework.Results.Errors;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Application.Extensions;

public static class ErrorExtension
{
    public static OAuth2ErrorResponse ToOAuth2ErrorResponse(this Error error)
    {
        return new OAuth2ErrorResponse
        {
            Error = error.Type,
            Description = error.Message
        };
    }
}
