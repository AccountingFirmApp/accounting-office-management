using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAccountingFirmRepository : IGenericRepository<Accountingfirm>
    {
        AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetFirmWithCompaniesAsync(int firmId);
        AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetFirmWithWorkersAsync(int firmId);
        AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetFirmWithAllDetailsAsync(int firmId);
    }
}