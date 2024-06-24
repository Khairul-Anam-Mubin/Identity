using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Extensions;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Errors;

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
            return OAuthError.InvalidResponseType.Result().ToObjectResult(ErrorResponseType.OAuth2Error);
        }

        var result = await _commandExecutor.ExecuteAsync(command);
        
        return result.ToObjectResult(ErrorResponseType.OAuth2Error);
    }

    [HttpPost]
    [Route("OAuth2/Token")]
    public async Task<IActionResult> CreateTokenAsync(TokenRequest request)
    {
        var command = request.ToCreateTokenByGrantTypeCommand();
        
        if (command is null)
        {
            return OAuthError.InvalidGrant.Result().ToObjectResult(ErrorResponseType.OAuth2Error);
        }

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToObjectResult(ErrorResponseType.OAuth2Error);
    }
}
