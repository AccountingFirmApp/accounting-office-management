using AccountingSystem.Domain.Entities;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        AccountingSystem.Domain.Entities.Task<Role?> GetByNameAsync(string name);
    }
}