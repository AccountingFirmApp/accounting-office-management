using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAccountingFirmRepository : IGenericRepository<Accountingfirm>
    {
        Task<Accountingfirm?> GetFirmWithCompaniesAsync(Guid firmId);
        Task<Accountingfirm?> GetFirmWithWorkersAsync(Guid firmId);
        Task<Accountingfirm?> GetFirmWithAllDetailsAsync(Guid firmId);
    }
}