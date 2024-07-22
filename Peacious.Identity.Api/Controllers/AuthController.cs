using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Extensions;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Infrastructure.Extensions;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ICommandExecutor commandExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;

    [HttpPost]
    [Route("OAuth2/Authorize")]
    [Authorize]
    public async Task<IActionResult> AuthorizeAsync(AuthorizationRequest request)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimType.UserId)?.Value;

        var command = request.ToAuthorizationResponseTypeCommand(userId!);

        if (command is null)
        {
            return OAuthError.InvalidResponseType.ToOAuth2ActionResult();
        }

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToOAuth2ActionResult();
    }

    [HttpPost]
    [Route("OAuth2/Token")]
    public async Task<IActionResult> CreateTokenAsync(TokenRequest request)
    {
        var command = request.ToCreateTokenByGrantTypeCommand();
        
        if (command is null)
        {
            return OAuthError.InvalidGrant.ToOAuth2ActionResult();
        }

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToOAuth2ActionResult();
    }
}
