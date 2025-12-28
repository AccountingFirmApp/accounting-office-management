using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyWorkerRepository : IGenericRepository<Companyworker>
    {
        Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(Guid workerId);
        Task<bool> AssignmentExistsAsync(Guid companyId, Guid workerId);
    }
}