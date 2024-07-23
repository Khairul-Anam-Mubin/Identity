using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Framework.Results.Errors.Adapters;
using Peacious.Framework.Results.Errors.Strategies;
using Peacious.Identity.Infrastructure.ErrorAdapters;

namespace Peacious.Identity.Infrastructure.Extensions;

public static class ErrorExtension
{
    public static IActionResult ToDefaultActionResult(this Error error)
        => error.ToActionResult(
            new DefaultErrorActionResultAdapter(DefaultErrorStatusCodeStrategy.Instance));

    public static IActionResult ToStandardActionResult(this Error error)
        => error.ToActionResult(
            new ProblemDetailsErrorActionResultAdapter(DefaultErrorStatusCodeStrategy.Instance));

    public static IActionResult ToOAuth2ActionResult(this Error error)
        => error.ToActionResult(
            new OAuth2ErrorActionResultAdapter(DefaultErrorStatusCodeStrategy.Instance));
}
