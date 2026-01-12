using AccountingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IFrequencyRepository : IGenericRepository<Frequency>
    {
        Task<Frequency?> GetByNameAsync(string name);
    }
}