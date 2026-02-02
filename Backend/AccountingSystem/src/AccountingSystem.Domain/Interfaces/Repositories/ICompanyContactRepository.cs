using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyContactRepository : IGenericRepository<Companycontact>
    {
        System.Threading.Tasks.Task<IEnumerable<Companycontact>> GetContactsByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task<Companycontact?> GetPrimaryContactAsync(int companyId);
    }
}