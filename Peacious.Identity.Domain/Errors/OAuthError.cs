using Peacious.Framework.Results;

namespace Peacious.Identity.Domain.Errors;

public class OAuthError
{
    public static readonly Error InvalidRequest = 
        Error.Validation(
            "invalid_request", 
            "an invalid request parameter is given", 
            "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error NoAccess =
        Error.Validation(
            "NO_ACCESS",
            "an invalid request parameter is given",
            "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error InvalidClient =
       Error.Validation(
           "invalid_client",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error InvalidGrant =
        Error.Validation(
           "invalid_grant",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error InvalidScope =
        Error.Validation(
           "invalid_scope",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error UnauthorizedClient =
        Error.Validation(
           "unauthorized_client",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error UnsupportedGrantType =
        Error.Validation(
           "unsupported_grant_type",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error UnsupportedResponseType =
        Error.Validation(
           "unsupported_response_type",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error AccessDenied =
        Error.Validation(
           "access_denied",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error ServerError =
        Error.Failure(
           "server_error",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    public static readonly Error TemporarilyUnavailable =
        Error.Failure(
           "temporarily_unavailable",
           "an invalid request parameter is given",
           "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");
}
