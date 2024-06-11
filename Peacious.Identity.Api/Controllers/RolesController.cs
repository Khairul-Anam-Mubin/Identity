using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(
    ICommandExecutor commandExecutor,
    IQueryExecutor queryExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IQueryExecutor _queryExecutor = queryExecutor;
    
    [HttpPost]
    public async Task<IActionResult> CreateNewRoleAsync(RoleRequest request)
    {
        var command = new CreateRoleCommand(request.RoleName);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetRolesAsync()
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRolesAsync(Roles roles)
    {
        var command = new DeleteRolesCommand(roles.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{roleId}/Permissions/Add")]
    public async Task<IActionResult> AddPermissionsToRoleAsync([FromRoute] string roleId, [FromBody] Permissions permissions)
    {
        var command = new AddPermissionsToRoleCommand(roleId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{roleId}/Permissions/Remove")]
    public async Task<IActionResult> RemovePermissionsFromRoleAsync([FromRoute] string roleId, [FromBody] Permissions permissions)
    {
        var command = new RemovePermissionsFromRoleCommand(roleId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }
}
