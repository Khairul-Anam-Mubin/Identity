using Peacious.Framework.DSA;
using Peacious.Framework.DSA.DisjointSetUnion;
using Peacious.Framework.DSA.Graphs;
using Peacious.Identity.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Application.Services;

public class PermissionService(IPermissionRepository permissionRepository)
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    private readonly Graph _permissionGraph = new(10000);
    private readonly Compresser _compresser = new Compresser();
    private readonly DSU _dsu = new DSU();
    
    public async Task Init()
    {
        var permissionDependencies = await _permissionRepository.GetPermissionDependenciesAsync();
        
        _dsu.Initialize(10000);
        
        foreach (var dependency in permissionDependencies)
        {
            var u = _compresser.GetCompressedKey(dependency.ParentPermissionId);
            var v = _compresser.GetCompressedKey(dependency.PermissionId);

            _permissionGraph.AddEdge(u, v);

            if (_dsu.IsInSameSet(u, v))
            {
                Console.WriteLine("Cycle");
            }

            _dsu.Union(u, v);
        }
    }
}