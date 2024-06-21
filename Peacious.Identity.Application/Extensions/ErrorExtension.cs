using Peacious.Framework.Results;

namespace Peacious.Identity.Application.Extensions;

public static class ErrorExtension
{
    public static OAuth2ErrorResponse ToOAuth2ErrorResponse(this Error error)
    {
        return new OAuth2ErrorResponse
        {
            Error = error.Title,
            Description = error.Description,
            Uri = error.Uri,
        };
    }
}
