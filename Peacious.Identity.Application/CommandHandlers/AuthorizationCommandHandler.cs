using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Application.CommandHandlers;

public class AuthorizationCommandHandler : ICommandHandler<AuthorizationCommand>
{
    public async Task<IResult> Handle(AuthorizationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
