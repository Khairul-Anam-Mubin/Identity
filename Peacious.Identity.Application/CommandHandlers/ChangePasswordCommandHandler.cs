using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class ChangePasswordCommandHandler(IUserRepository userRepository) : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);

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
