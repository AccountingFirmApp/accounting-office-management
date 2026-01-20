using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ITaskTypeRepository : IGenericRepository<Tasktype>
    {
        AccountingSystem.Domain.Entities.Task<IEnumerable<Tasktype>> GetByCategoryAsync(string category);
    }
}