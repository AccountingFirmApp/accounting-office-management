using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;


namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAccountingFirmRepository : IGenericRepository<Accountingfirm>
    {

        System.Threading.Tasks.Task<Accountingfirm?> GetFirmWithCompaniesAsync(int firmId);
        System.Threading.Tasks.Task<Accountingfirm?> GetFirmWithWorkersAsync(int firmId);
        System.Threading.Tasks.Task<Accountingfirm?> GetFirmWithAllDetailsAsync(int firmId);
    }
}