using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyReportConfigRepository : IGenericRepository<Companyreportconfig>
    {
        AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(int companyId);
        AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync();
        AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetByCompanyIdAsync(int companyId);

    }
}