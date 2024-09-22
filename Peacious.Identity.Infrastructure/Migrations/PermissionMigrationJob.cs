using Peacious.Framework;
using Peacious.Framework.ORM.Migrations;
using Peacious.Framework.PermissionAuthorization;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;
using System.Reflection;

namespace Peacious.Identity.Infrastructure.Migrations;

public class PermissionMigrationJob(
    IPermissionRepository permissionRepository) : IMigrationJob
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task MigrateAsync()
    {
        var permissionTitles = new HashSet<string>();
        
        foreach (var assembly in AssemblyCache.Instance.GetAddedAssemblies())
        {
            foreach (var permissionTitle in GetPermissionTitles(assembly.GetExportedTypes()))
            {
                permissionTitles.Add(permissionTitle);
            }
        }

        var permissions = new List<Permission>();

        foreach (var permissionTitle in permissionTitles.ToList())
        {
            permissions.Add(Permission.Create(permissionTitle, true));    
        }

        await _permissionRepository.SaveAsync(permissions);
    }

    private List<string> GetPermissionTitles(Type[] types)
    {
        var permissionTitles = new List<string>();

        foreach (var type in types)
        {
            permissionTitles.AddRange(GetPermissionTitles(type.GetCustomAttributes<HasPermissionAttribute>()));
            permissionTitles.AddRange(GetPermissionTitles(type.GetMembers()));
        }

        return permissionTitles;
    }

    private List<string> GetPermissionTitles(MemberInfo[] members) 
    {
        var permissionTitles = new List<string>();

        foreach (var member in members)
        {
            permissionTitles.AddRange(GetPermissionTitles(member.GetCustomAttributes<HasPermissionAttribute>()));
        }

        return permissionTitles;
    }

    private List<string> GetPermissionTitles(IEnumerable<HasPermissionAttribute> hasPermissionAttributes)
    {
        if (!hasPermissionAttributes.Any())
        {
            return new List<string>();
        }

        var permissionTitles = new HashSet<string>();

        foreach (var hasPermissionAttribute in hasPermissionAttributes)
        {
            if (string.IsNullOrEmpty(hasPermissionAttribute.Policy))
            {
                continue;
            }

            permissionTitles.Add(hasPermissionAttribute.Policy);
        }

        return permissionTitles.ToList();
    }
}
