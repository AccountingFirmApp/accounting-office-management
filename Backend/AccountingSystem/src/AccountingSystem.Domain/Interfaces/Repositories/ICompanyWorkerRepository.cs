using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyWorkerRepository : IGenericRepository<Companyworker>
    {
        Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId);
        Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId);
        Task<bool> AssignmentExistsAsync(int companyId, int workerId);
    }
}