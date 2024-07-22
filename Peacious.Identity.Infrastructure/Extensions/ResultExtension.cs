using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Adapters;
using Peacious.Framework.Results.Errors.Adapters;
using Peacious.Framework.Results.Errors.Strategies;
using Peacious.Framework.Results.Strategies;
using Peacious.Identity.Infrastructure.ErrorAdapters;

namespace Peacious.Identity.Infrastructure.Extensions;

public static class ResultExtension
{
    public static IActionResult ToDefaultActionResult(this IResult result)
    {
        return result.ToActionResult(
            new DefaultActionResultAdapter(new DefaultStatusCodeStrategy()), 
            new DefaultErrorActionResultAdapter(new DefaultErrorStatusCodeStrategy()));
    }

    public static IActionResult ToStandardActionResult(this IResult result)
    {
        return result.ToActionResult(
            new DefaultActionResultAdapter(new DefaultStatusCodeStrategy()),
            new ProblemDetailsErrorActionResultAdapter(new DefaultErrorStatusCodeStrategy()));
    }

    public static IActionResult ToOAuth2ActionResult(this IResult result)
    {
        return result.ToActionResult(
            new DefaultActionResultAdapter(new DefaultStatusCodeStrategy()),
            new OAuth2ErrorActionResultAdapter(new DefaultErrorStatusCodeStrategy()));
    }
}
