using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Results;
using Peacious.Identity.Domain.Events;
using Peacious.Identity.Domain.ValueObjects;
using System.Security.Claims;

namespace Peacious.Identity.Domain.Entities;

public class User : Entity, IRepositoryItem
{
    public Name Name { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public string UserName { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User(Name name, Email email, Password password, string userName)
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
        Email = email;
        UserName = userName;
        Password = password;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string firstName, string lastName, string emailAddress, string plainPassword)
    {
        var name = new Name(firstName, lastName);
        var email = Email.Create(emailAddress);
        var password = Password.Create(plainPassword);
        var userName = emailAddress;
        var user = new User(name, email, password, userName);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id, user.UserName));

        return user;
    }

    public void VerifyEmail()
    {
        Email.Verify();
    }

    public IResult ChangeName(string firstName, string lastName)
    {
        Name = new Name(firstName, lastName);

        return Result.Success();
    }

    public IResult ChangePassword(string oldPassword, string newPassword)
    {
        if (!Password.IsMatch(oldPassword))
        {
            return Result.Error("Old password not matched");
        }

        Password = Password.Create(newPassword);

        return Result.Success();
    }

    public IResult ResetPassword(string newPassword)
    {
        Password = Password.Create(newPassword);

        return Result.Success();
    }

    public IResult ChangeUserName(string userName)
    {
        UserName = userName;

        return Result.Success();
    }

    public List<Claim> ToClaims()
    {
        return new List<Claim>
        {
            new Claim("sub", Id),
            new Claim("username", UserName),
            new Claim("first_name", Name.FirstName),
            new Claim("last_name", Name.LastName),
        };
    }
}
