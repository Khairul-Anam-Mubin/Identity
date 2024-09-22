using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Contracts.Models;

namespace Peacious.Identity.Infrastructure.CommandAdpters;

public interface ITokenRequestToCommandConverter
{
    ICommand<TokenResponse>? TryConvert(TokenRequest tokenRequest);
}

public abstract class TokenRequestToCommandConverter : ITokenRequestToCommandConverter
{
    private ITokenRequestToCommandConverter? NextConverter { get; set; }
    
    protected abstract bool CanConvert(TokenRequest tokenRequest);
    protected abstract ICommand<TokenResponse>? Convert(TokenRequest tokenRequest);
    
    public ITokenRequestToCommandConverter SetNext(ITokenRequestToCommandConverter nextConverter)
    {
        NextConverter = nextConverter;
        return this;
    }

    public ICommand<TokenResponse>? TryConvert(TokenRequest tokenRequest)
    {
        if (CanConvert(tokenRequest)) return Convert(tokenRequest);

        return NextConverter?.TryConvert(tokenRequest);
    }
}