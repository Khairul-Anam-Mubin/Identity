using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class ChangeNameCommandHandler(IUserRepository userRepository) : ICommandHandler<ChangeNameCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult> Handle(ChangeNameCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Error.NotFound("CurrentUser not found").Result();
        }

        var firstName = command.FirstName;
        var lastName = command.LastName;

        if (string.IsNullOrEmpty(firstName))
        {
            firstName = user.Name.FirstName;
        }
        
        if (string.IsNullOrEmpty(lastName))
        {
            lastName = user.Name.LastName;
        }

        if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
        {
            user.ChangeName(firstName, lastName);
        }
       
        if (!string.IsNullOrEmpty(command.UserName))
        {
            user.ChangeUserName(command.UserName);
        }

        await _userRepository.SaveAsync(user);

        return Result.Success("Name Changed");
    }
}
