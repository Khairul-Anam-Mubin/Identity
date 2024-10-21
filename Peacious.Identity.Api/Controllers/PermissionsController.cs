using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Queries;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Infrastructure.Extensions;

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
    [Authorize]
    public async Task<IActionResult> CreatePermissionAsync(PermissionRequest request)
    {
        var command = new CreatePermissionCommand(request.Title, request.IsCustom);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPermissionsAsync()
    {
        var query = new PermissionsQuery();

        var result = await _queryExecutor.ExecuteAsync(query);

        return result.ToStandardActionResult();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePermissionsAsync(Permissions permissions)
    {
        var command = new DeletePermissionsCommand(permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }
}
