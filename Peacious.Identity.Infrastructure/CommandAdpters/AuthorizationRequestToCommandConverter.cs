using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.DTOs;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public interface IAuthorizationRequestToCommandConverter
{
    ICommand<AuthorizationResponse>? TryConvert(AuthorizationRequest request);
}

public abstract class AuthorizationRequestToCommandConverter : IAuthorizationRequestToCommandConverter
{
    private IAuthorizationRequestToCommandConverter? NextConverter { get; set; }

    protected abstract bool CanConvert(AuthorizationRequest request);
    protected abstract ICommand<AuthorizationResponse>? Convert(AuthorizationRequest request);

    public IAuthorizationRequestToCommandConverter SetNext(IAuthorizationRequestToCommandConverter nextConverter)
    {
        NextConverter = nextConverter;
        return this;
    }

    public ICommand<AuthorizationResponse>? TryConvert(AuthorizationRequest request)
    {
        if (CanConvert(request)) return Convert(request);

        return NextConverter?.TryConvert(request);
    }
}
