using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyReportConfigRepository : IGenericRepository<Companyreportconfig>
    {
        Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync();
    }
}