using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyWorkerRepository : IGenericRepository<Companyworker>
    {
        System.Threading.Tasks.Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId);
        System.Threading.Tasks.Task<bool> AssignmentExistsAsync(int companyId, int workerId);
    }
}