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
public class UsersController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly IQueryExecutor _queryExecutor = queryExecutor;

    [HttpPost]
    public async Task<IActionResult> UserRegisterAsync(UserRegistrationRequest request)
    {
        var command = new UserRegisterCommand(
            request.FirstName, 
            request.LastName, 
            request.UserName,
            request.Email, 
            request.Password);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpGet]
    [Route("Info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfoAsyc()
    {
        var query = new UserInfoQuery();

        var result = await _queryExecutor.ExecuteAsync(query);

        return result.ToStandardActionResult();
    }

    [HttpGet]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string email, [FromQuery] string code)
    {
        var command = new ConfirmEmailCommand(email, code);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpPost]
    [Route("ResendConfirmationEmail")]
    public async Task<IActionResult> ResendConfirmationEmailAsync([FromBody] ResendConfirmationEmailRequest request)
    {
        // TODO
        return Ok();
    }

    #region Passwords

    [HttpPost]
    [Route("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var command = new ChangePasswordCommand(
            request.OldPassword,
            request.NewPassword);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgetPasswordAsync(ForgetPasswordRequest request)
    {
        var command = new ForgetPasswordCommand(request.UserName);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        // TODO
        return Ok();
    }

    #endregion

    #region Permissions

    [HttpPost]
    [Route("{userId}/Permissions")]
    public async Task<IActionResult> AddUserPermissionsAsync(string userId, [FromBody] Permissions permissions)
    {
        var command = new AddUserPermissionsCommand(userId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpDelete]
    [Route("{userId}/Permissions")]
    public async Task<IActionResult> RemoveUserPermissionsAsync(string userId, [FromBody] Permissions permissions)
    {
        var command = new RemoveUserPermissionsCommand(userId, permissions.Ids);

        var result = await _commandExecutor.ExecuteAsync(command);

        return result.ToStandardActionResult();
    }

    [HttpGet]
    [Route("{userId}/Permissions")]
    public async Task<IActionResult> GetUserPermissionsAsync([FromRoute] string userId)
    {
        var query = new UserPermissionsQuery()
        {
            UserId = userId
        };

        var result = await _queryExecutor.ExecuteAsync(query);

        return result.ToStandardActionResult();
    }

    #endregion
}
