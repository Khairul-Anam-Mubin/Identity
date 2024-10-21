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
            return Error.Conflict($"Email Address : {request.Email} already exist!").Result();
        }

        var isUserNameExists = await _userRepository.IsUserEmailExistAsync(request.UserName);

        if (isUserNameExists)
        {
            return Error.Conflict($"Username : {request.UserName} already exist!").Result();
        }

        var user = User.Create(
            request.FirstName, 
            request.LastName,
            request.UserName,
            request.Email, 
            request.Password);

        if (await _userRepository.SaveAsync(user)) 
        {
            return Result.Success("Registered Successfully");
        }

        return Error.Failure("save_problem").Result();
    }
}
