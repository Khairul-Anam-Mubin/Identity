using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.Results.Errors;
using Peacious.Framework.Results.Errors.Adapters;
using Peacious.Identity.Infrastructure.ErrorAdapters;

namespace Peacious.Identity.Infrastructure.Extensions;

public static class ErrorExtension
{
    public static IActionResult ToDefaultActionResult(this Error error)
    {
        return error.ToActionResult(DefaultErrorActionResultAdapter.Instance);
    }

    public static IActionResult ToStandardActionResult(this Error error)
    {
        return error.ToActionResult(ProblemDetailsErrorActionResultAdapter.Instance);
    }

    public static IActionResult ToOAuth2ActionResult(this Error error)
    {
        return error.ToActionResult(OAuth2ErrorActionResultAdapter.Instance);
    }
}
