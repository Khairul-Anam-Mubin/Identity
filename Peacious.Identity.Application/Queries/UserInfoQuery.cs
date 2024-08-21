using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Application.Queries;

public record UserInfoQuery : IQuery<UserInfo>
{
}
