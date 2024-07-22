using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.Results.Errors;
using Peacious.Framework.Results.Errors.Adapters;
using Peacious.Framework.Results.Errors.Strategies;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Infrastructure.ErrorAdapters;

public class OAuth2ErrorActionResultAdapter(
    IErrorStatusCodeStrategy errorStatusCodeStrategy) : IErrorActionResultAdapter
{
    private readonly IErrorStatusCodeStrategy _errorStatusCodeStrategy = errorStatusCodeStrategy;

    public IActionResult Convert(Error error)
    {
        var errorResponse = new OAuth2ErrorResponse
        {
            Error = error.Title!,
            Description = error.Description,
            Uri = error.Uri
        };

        return new ObjectResult(errorResponse)
        {
            StatusCode = _errorStatusCodeStrategy.GetErrorStatusCode(error.Type)
        };
    }
}
