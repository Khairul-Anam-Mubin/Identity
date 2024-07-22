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
    {
        return error.ToActionResult(new DefaultErrorActionResultAdapter(new DefaultErrorStatusCodeStrategy()));
    }

    public static IActionResult ToStandardActionResult(this Error error)
    {
        return error.ToActionResult(new ProblemDetailsErrorActionResultAdapter(new DefaultErrorStatusCodeStrategy()));
    }

    public static IActionResult ToOAuth2ActionResult(this Error error)
    {
        return error.ToActionResult(new OAuth2ErrorActionResultAdapter(new DefaultErrorStatusCodeStrategy()));
    }
}
