namespace Peacious.Identity.Domain.ValueObjects;

public record Email
{
    public string UserName { get; private set; }
    public string Domain { get; private set; }
    public bool IsVerified { get; private set; }

    private Email(string userName, string domain)
    {
        UserName = userName;
        Domain = domain;
        IsVerified = false;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public string GetEmail()
    {
        return $"{UserName}@{Domain}";
    }

    public string GetUniqueUserName()
    {
        var domainNameToInteger = Domain.GetHashCode() % 10000;

        return $"{UserName}{domainNameToInteger}";
    }

    public static Email Create(string email)
    {
        var splittedEmail = email.Split('@');

        var userName = splittedEmail.First();
        var domain = splittedEmail.Last();

        return new Email(userName, domain);
    }
}