using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController(
    ICommandExecutor commandExecutor,
    IQueryExecutor queryExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IQueryExecutor _queryExecutor = queryExecutor;

    [HttpPost]
    public async Task<IActionResult> CreatePermissionAsync(PermissionRequest request)
    {
        var command = new CreatePermissionCommand(request.Title);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissionsAsync()
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePermissionsAsync(Permissions permissions)
    {
        var command = new DeletePermissionsCommand(permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{permissionId}/Add-Permissions")]
    public async Task<IActionResult> AddPermissionsToPermissionAsync([FromRoute] string permissionId, [FromBody] Permissions permissions)
    {
        var command = new AddChildPermissionsCommand(permissionId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{permissionId}/Remove-Permissions")]
    public async Task<IActionResult> RemovePermissionsFromPermissionAsync([FromRoute] string permissionId, [FromBody] Permissions permissions)
    {
        var command = new RemoveChildPermissionsCommand(permissionId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }
}
