﻿using Peacious.Framework.Results.Errors;

namespace Peacious.Identity.Domain.Errors;

public class OAuthError
{
    #region CommentedError
    //public static Error InvalidUser(string userName) => 
    //    Error.NotFound(
    //        "invalid_user",
    //        $"User with username : {userName} not found.");

    //public static Error InvalidClient(string clientId) =>
    //   Error.NotFound(
    //       "invalid_client",
    //       $"Client with client_id : {clientId} not found.",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static Error InvalidCredentials =>
    //    Error.Unauthorized(
    //        "unauthorized_client",
    //        $"Invalid client or user credentials.");

    //public static Error InvalidToken(string description) =>
    //    Error.Unauthorized(
    //        "invalid_token",
    //        description);

    //public static Error InvalidRequest(string parameter) => 
    //    Error.Validation(
    //        "invalid_request", 
    //        $"Invalid {parameter}", 
    //        "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error NoAccess =
    //    Error.Validation(
    //        "NO_ACCESS",
    //        "an invalid request parameter is given",
    //        "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error InvalidGrant =
    //    Error.Validation(
    //       "invalid_grant",
    //       "Please provide a valid grant type.",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error InvalidResponseType =
    //    Error.Validation(
    //       "invalid_response_type",
    //       "Please provide a valid response type.",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error InvalidScope =
    //    Error.Validation(
    //       "invalid_scope",
    //       "an invalid request parameter is given",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error UnauthorizedClient =
    //    Error.Unauthorized(
    //       "unauthorized_client",
    //       "Client is not authorized",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error UnsupportedGrantType =
    //    Error.Validation(
    //       "unsupported_grant_type",
    //       "Grant type not supported. Please try with another grant.",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error UnsupportedResponseType =
    //    Error.Validation(
    //       "unsupported_response_type",
    //       "an invalid request parameter is given",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error AccessDenied =
    //    Error.Validation(
    //       "access_denied",
    //       "an invalid request parameter is given",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error ServerError =
    //    Error.Failure(
    //       "server_error",
    //       "Some exceptions needs to be handled. Please contact for support.",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    //public static readonly Error TemporarilyUnavailable =
    //    Error.Failure(
    //       "temporarily_unavailable",
    //       "an invalid request parameter is given",
    //       "https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/");

    #endregion

    public static Error InvalidUser(string userName) =>
        Error.NotFound(
            $"User with username : {userName} not found.");

    public static Error InvalidClient(string clientId) =>
       Error.NotFound(
           $"Client with client_id : {clientId} not found.");

    public static Error InvalidCredentials =>
        Error.Unauthorized(
            $"Invalid client or user credentials.");

    public static Error InvalidToken(string description) =>
        Error.Unauthorized(
            description);

    public static Error InvalidRequest(string parameter) =>
        Error.Validation(
            $"Invalid {parameter}");

    public static readonly Error NoAccess =
        Error.Validation(
            "an invalid request parameter is given");

    public static readonly Error InvalidGrant =
        Error.Validation(
           "Please provide a valid grant type.");

    public static readonly Error InvalidResponseType =
        Error.Validation(
           "Please provide a valid response type.");

    public static readonly Error InvalidScope =
        Error.Validation(
           "an invalid request parameter is given");

    public static readonly Error UnauthorizedClient =
        Error.Unauthorized(
           "Client is not authorized");

    public static readonly Error UnsupportedGrantType =
        Error.Validation(
           "Grant type not supported. Please try with another grant.");

    public static readonly Error UnsupportedResponseType =
        Error.Validation(
           "an invalid request parameter is given");

    public static readonly Error AccessDenied =
        Error.Validation(
           "an invalid request parameter is given");

    public static readonly Error ServerError =
        Error.Failure(
           "Some exceptions needs to be handled. Please contact for support.");

    public static readonly Error TemporarilyUnavailable =
        Error.Failure(
           "an invalid request parameter is given");
}
