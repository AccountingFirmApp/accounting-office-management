using AccountingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IFrequencyRepository : IGenericRepository<Frequency>
    {
        System.Threading.Tasks.Task<Frequency?> GetByNameAsync(string name);
    }
}