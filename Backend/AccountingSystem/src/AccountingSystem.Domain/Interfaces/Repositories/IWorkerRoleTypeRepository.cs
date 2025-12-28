using AccountingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IWorkerRoleTypeRepository : IGenericRepository<Workerroletype>
    {
        Task<Workerroletype?> GetByNameAsync(string name);
    }
}