using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyContactRepository : IGenericRepository<Companycontact>
    {
        AccountingSystem.Domain.Entities.Task<IEnumerable<Companycontact>> GetContactsByCompanyIdAsync(int companyId);
        AccountingSystem.Domain.Entities.Task<Companycontact?> GetPrimaryContactAsync(int companyId);
    }
}