namespace Peacious.Identity.Domain.ValueObjects;

public record Email
{
    public string Address { get; private set; }
    public bool IsVerified { get; private set; }

    private Email(string email)
    {
        Address = email;
        IsVerified = false;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public static Email Create(string email)
    {
        return new Email(email);
    }
}