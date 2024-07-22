using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Framework.Results.Errors.Adapters;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Infrastructure.ErrorAdapters;

public class OAuth2ErrorActionResultAdapter : IErrorActionResultAdapter
{
    private static readonly object _lockObject = new();
    private static IErrorActionResultAdapter? _instance;

    public static IErrorActionResultAdapter Instance
    {
        get
        {
            if (_instance is not null)
            {
                return _instance;
            }
            lock (_lockObject)
            {
                _instance ??= new OAuth2ErrorActionResultAdapter();
            }
            return _instance;
        }
    }

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
            StatusCode = StatusCodeProvider.GetStatusCode(error.Type)
        };
    }
}
