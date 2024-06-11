using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController(
    ICommandExecutor commandExecutor, 
    IQueryExecutor queryExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IQueryExecutor _queryExecutor = queryExecutor;

    [HttpPost]
    [Route("Credential")]
    public async Task<IActionResult> CreateClientCredentialAsync(ClientCredentialRequest request)
    {
        var command = new CreateClientCredentialCommand(
            request.ClientName,
            request.ClientWebsite,
            request.LogoUrl,
            request.RedirectUri,
            request.PermissionIds);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{clientId}/Secret/Revoke")]
    public async Task<IActionResult> RevokeClientSecretAsync(string clientId)
    {
        var command = new RevokeClientSecretCommand(clientId);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{clientId}/Secret/Generate-New")]
    public async Task<IActionResult> GenerateNewClientSecretAsync(string clientId)
    {
        var command = new GenerateNewClientSecretCommand(clientId);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }
}
