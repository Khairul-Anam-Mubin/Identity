namespace Peacious.Identity.Domain.ValueObjects;

public record Name
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    internal Name(string firstName, string lastName)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}