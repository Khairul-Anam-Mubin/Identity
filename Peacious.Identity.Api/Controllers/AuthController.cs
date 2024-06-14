using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    ICommandExecutor commandExecutor,
    IQueryExecutor queryExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IQueryExecutor _queryExecutor = queryExecutor;

    [HttpPost]
    [Route("OAuth2/Authorize")]
    public async Task<IActionResult> AuthorizeAsync([FromBody]AuthorizationRequest request)
    {
        var command = new AuthorizationCommand(
            request.ResponseType,
            request.ClientId,
            request.RedirectUri,
            request.Scope,
            request.State,
            request.CodeChallange,
            request.CodeChallangeMethod);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("OAuth2/Token")]
    public async Task<IActionResult> CreateTokenAsync(TokenRequest request)
    {
        var command = new CreateTokenCommand(
            request.GrantType,
            request.ClientId,
            request.UserName,
            request.Password,
            request.RefreshToken,
            request.ClientSecret,
            request.Code,
            request.RedirectUri,
            request.CodeVerifier);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }
}
