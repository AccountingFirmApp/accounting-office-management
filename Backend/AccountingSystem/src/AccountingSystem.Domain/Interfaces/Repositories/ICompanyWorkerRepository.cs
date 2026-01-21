using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyWorkerRepository : IGenericRepository<Companyworker>
    {
        AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId);
        AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId);
        AccountingSystem.Domain.Entities.Task<bool> AssignmentExistsAsync(int companyId, int workerId);
    }
}