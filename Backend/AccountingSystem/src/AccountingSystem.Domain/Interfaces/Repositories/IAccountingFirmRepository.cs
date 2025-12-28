using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAccountingFirmRepository : IGenericRepository<Accountingfirm>
    {
        Task<Accountingfirm?> GetFirmWithCompaniesAsync(int firmId);
        Task<Accountingfirm?> GetFirmWithWorkersAsync(int firmId);
        Task<Accountingfirm?> GetFirmWithAllDetailsAsync(int firmId);
    }
}