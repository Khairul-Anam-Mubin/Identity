using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class ChangeNameCommandHandler : ICommandHandler<ChangeNameCommand>
{
    public async Task<IResult> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("Name Changed");
    }
}
