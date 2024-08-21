using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Queries;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.QueryHandlers;

public class UserInfoQueryHandler(
    IUserRepository userRepository,
    IUserScopeContext userScopeContext) : IQueryHandler<UserInfoQuery, UserInfo>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserScopeContext _userScopeContext = userScopeContext;

    public async Task<IResult<UserInfo>> Handle(UserInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = _userScopeContext.User.Id;

        var user = await _userRepository.GetByIdAsync(userId!);

        if (user is null)
        {
            return Error.NotFound("User Not Found").Result<UserInfo>();
        }

        var userInfo = new UserInfo(
            user.Id,
            user.UserName,
            user.Name.FirstName,
            user.Name.LastName,
            user.Email.Address);

        return Result.Success(userInfo);
    }
}
