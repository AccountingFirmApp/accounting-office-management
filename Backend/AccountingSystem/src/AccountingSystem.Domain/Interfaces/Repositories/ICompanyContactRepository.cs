using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyContactRepository : IGenericRepository<Companycontact>
    {
        Task<IEnumerable<Companycontact>> GetContactsByCompanyIdAsync(int companyId);
        Task<Companycontact?> GetPrimaryContactAsync(int companyId);
    }
}