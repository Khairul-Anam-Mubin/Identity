using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Framework.PermissionAuthorization;
using Peacious.Identity.Application.Extensions;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Infrastructure.Extensions;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    ICommandExecutor commandExecutor, IUserScopeContext userScopeContext) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IUserScopeContext _userScopeContext = userScopeContext;

    [HttpPost]
    [Route("OAuth2/Authorize")]
    [Authorize]
    public async Task<IActionResult> AuthorizeAsync(AuthorizationRequest request)
    {
        var userId = _userScopeContext.User?.Id!;

        var command = request.ToAuthorizationResponseTypeCommand(userId);

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
