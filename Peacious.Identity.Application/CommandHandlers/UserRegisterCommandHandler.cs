using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
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
            return Result.Error($"Email : {request.Email} already exist!");
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

        return Result.Error("Register Failed");
    }
}
