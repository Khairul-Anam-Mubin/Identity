using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Extensions;
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
        var command = request.ToAuthorizationResponseTypeCommand();

        if (command is null)
        {
            return BadRequest($"Response Type : {request.ResponseType} Not Supported.");
        }

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("OAuth2/Token")]
    public async Task<IActionResult> CreateTokenAsync(TokenRequest request)
    {
        var command = request.ToCreateTokenByGrantTypeCommand();
        
        if (command is null)
        {
            return BadRequest($"{request.GrantType} : Grant Type Not Supported.");
        }

        var result = await _commandExecutor.ExecuteAsync(command);

        if (result.IsFailure())
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }
}
