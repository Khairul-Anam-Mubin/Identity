using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peacious.Framework.CQRS;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Infrastructure.Extensions;

namespace Peacious.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
    ICommandExecutor commandExecutor, 
    IQueryExecutor queryExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IQueryExecutor _queryExecutor = queryExecutor;

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> UserRegisterAsync(UserRegistrationRequest request)
    {
        var command = new UserRegisterCommand(
            request.FirstName, 
            request.LastName, 
            request.Email, 
            request.Password);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToDefaultActionResult();
    }

    [HttpGet]
    [Route("Info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfoAsyc()
    {
        return Ok("No Info for now");
    }

    [HttpPost]
    [Route("Address/{email}/Verify")]
    public async Task<IActionResult> VerifyUserEmailAsync(string email, [FromQuery] string code)
    {
        var command = new VerifyUserEmailCommand(email, code);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("Name/Change")]
    [Authorize]
    public async Task<IActionResult> ChangeNameAsync(ChangeNameRequest request)
    {
        var userId = User.Claims.First(x => x.Type == ClaimType.UserId).Value;

        var command = new ChangeNameCommand(
            userId,
            request.FirstName, 
            request.LastName, 
            request.UserName);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpPost]
    [Route("Password/Change")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userId = User.Claims.First(x => x.Type == ClaimType.UserId).Value;

        var command = new ChangePasswordCommand(
            userId,
            request.OldPassword,
            request.NewPassword);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("Password/Forget")]
    public async Task<IActionResult> ForgetPasswordAsync(ForgetPasswordRequest request)
    {
        var command = new ForgetPasswordCommand(request.UserName);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{userId}/Roles/Add")]
    public async Task<IActionResult> AddUserRolesAsync(string userId,[FromBody] Roles roles)
    {
        var command = new AddUserRolesCommand(userId, roles.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{userId}/Roles/Remove")]
    public async Task<IActionResult> RemoveUserRolesAsync(string userId, [FromBody] Roles roles)
    {
        var command = new RemoveUserRolesCommand(userId, roles.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{userId}/Permissions/Add")]
    public async Task<IActionResult> AddUserPermissionsAsync(string userId, [FromBody] Permissions permissions)
    {
        var command = new AddUserPermissionsCommand(userId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }

    [HttpPost]
    [Route("{userId}/Permissions/Remove")]
    public async Task<IActionResult> RemoveUserPermissionsAsync(string userId, [FromBody] Permissions permissions)
    {
        var command = new RemoveUserPermissionsCommand(userId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return Ok(result);
    }
}
