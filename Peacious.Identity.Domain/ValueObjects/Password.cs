using Peacious.Framework.Security;

namespace Peacious.Identity.Domain.ValueObjects;

public record Password
{
    public string Hash { get; private set; }
    public string Salt { get; private set; }

    protected Password(string password, int saltKeySize)
    {
        Salt = PasswordHelper.GenerateRandomSalt(saltKeySize);
        Hash = PasswordHelper.GetPasswordHash(password, Salt);
    }

    public bool IsMatch(string password)
    {
        var passwordHash = PasswordHelper.GetPasswordHash(password, Salt);
        return Hash.Equals(passwordHash);
    }

    public static Password Create(string password, int saltKeySize = 64)
    {
        return new Password(password, saltKeySize);
    }
}
