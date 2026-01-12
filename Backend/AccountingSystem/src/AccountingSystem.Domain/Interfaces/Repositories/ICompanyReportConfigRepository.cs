using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyReportConfigRepository : IGenericRepository<Companyreportconfig>
    {
        Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(int companyId);
        Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync();
        Task<IEnumerable<Companyreportconfig>> GetByCompanyIdAsync(int companyId);

    }
}