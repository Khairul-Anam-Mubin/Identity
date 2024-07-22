using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Adapters;
using Peacious.Framework.Results.Errors.Adapters;
using Peacious.Identity.Infrastructure.ErrorAdapters;

namespace Peacious.Identity.Infrastructure.Extensions;

public static class ResultExtension
{
    public static IActionResult ToDefaultActionResult(this IResult result)
    {
        return result.ToActionResult(
            DefaultActionResultAdapter.Instance, 
            DefaultErrorActionResultAdapter.Instance);
    }

    public static IActionResult ToStandardActionResult(this IResult result)
    {
        return result.ToActionResult(
            DefaultActionResultAdapter.Instance,
            ProblemDetailsErrorActionResultAdapter.Instance);
    }

    public static IActionResult ToOAuth2ActionResult(this IResult result)
    {
        return result.ToActionResult(
            DefaultActionResultAdapter.Instance,
            OAuth2ErrorActionResultAdapter.Instance);
    }
}
