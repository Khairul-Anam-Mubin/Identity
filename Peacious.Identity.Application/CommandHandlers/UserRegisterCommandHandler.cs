using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class UserRegisterCommandHandler(IUserRepository userRepository) : ICommandHandler<UserRegisterCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<IResult> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailExist = await _userRepository.IsUserEmailExistAsync(request.Email);

        if (isEmailExist)
        {
            return Error.Conflict("email_exist", $"Email Address : {request.Email} already exist!").Result();
        }

        var user = User.Create(
            request.FirstName, 
            request.LastName,
            request.Email, 
            request.Password);

        if (await _userRepository.SaveAsync(user)) 
        {
            return Result.Success("Registered Successfully");
        }

        return Error.Failure("save_problem").Result();
    }
}
