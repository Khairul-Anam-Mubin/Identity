using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Infrastructure.CommandAdpters;
using Peacious.Identity.Infrastructure.Extensions;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    ICommandExecutor commandExecutor,
    ITokenRequestToCommandConverter tokenRequestToCommandConverter,
    IAuthorizationRequestToCommandConverter authorizationRequestToCommandConverter) : ControllerBase
{
    [HttpPost]
    [Route("OAuth2/Authorize")]
    [Authorize]
    public async Task<IActionResult> AuthorizeAsync(AuthorizationRequest request)
    {
        var command =
           authorizationRequestToCommandConverter.TryConvert(request);

        if (command is null)
        {
            return OAuthError.InvalidResponseType.ToOAuth2ActionResult();
        }

        var result = await commandExecutor.ExecuteAsync(command);

        return result.ToOAuth2ActionResult();
    }

    [HttpPost]
    [Route("OAuth2/Token")]
    public async Task<IActionResult> CreateTokenAsync(TokenRequest request)
    {
        var command = tokenRequestToCommandConverter.TryConvert(request);

        if (command is null)
        {
            return OAuthError.InvalidGrant.ToOAuth2ActionResult();
        }

        var result = await commandExecutor.ExecuteAsync(command);

        return result.ToOAuth2ActionResult();
    }
}
