using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;

namespace Peacious.Identity.Domain.Entities;

public class Permission : Entity, IRepositoryItem
{
    public string Title { get; private set; }
    public bool IsCustom { get; private set; }

    private Permission(string title, bool isCustom) 
        : base(Guid.NewGuid().ToString())
    {
        Title = title;
        IsCustom = isCustom;
    }

    public static Permission Create(string title, bool isCustom)
    {
        return new Permission(title, isCustom);
    }
}
