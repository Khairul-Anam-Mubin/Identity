using MediatR;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Repositories;
using Peacious.Identity.Domain.ValueObjects;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForPasswordGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository) : ICommandHandler<CreateTokenForPasswordGrantTypeCommand>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<IResult> Handle(CreateTokenForPasswordGrantTypeCommand command, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return Result.Error("Invalid Client Id");
        }

        var user = await _userRepository.GetUserByUserNameAsync(command.UserName);

        if (user is null)
        {
            return Result.Error("User not found.");
        }

        if (!user.Password.IsMatch(command.Password))
        {
            return Result.Error("Password Error.");
        }

        return Result.Success();
    }
}
