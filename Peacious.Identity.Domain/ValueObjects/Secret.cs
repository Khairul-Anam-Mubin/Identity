namespace Peacious.Identity.Domain.ValueObjects;

public record Secret : Password
{
    private Secret(string secret, int saltKeySize) : base(secret, saltKeySize) {}

    public static new Secret Create(string secret, int saltKeySize = 64)
    {
        return new Secret(secret, saltKeySize);
    }
}
