using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IUserScopeContext userScopeContext) : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserScopeContext _userScopeContext = userScopeContext;

    public async Task<IResult> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var userId = _userScopeContext.User.Id;

        var user = await _userRepository.GetByIdAsync(userId!);

        if (user is null)
        {
            return Error.NotFound("CurrentUser not found").Result();
        }

        var passwordChangeResult = user.ChangePassword(command.OldPassword, command.NewPassword);

        if (passwordChangeResult.IsFailure)
        {
            return passwordChangeResult;
        }

        await _userRepository.SaveAsync(user);

        return Result.Success("Password Changed");
    }
}
