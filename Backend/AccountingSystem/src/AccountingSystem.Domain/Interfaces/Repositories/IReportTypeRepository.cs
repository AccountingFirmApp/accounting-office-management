using AccountingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IReportTypeRepository : IGenericRepository<Reporttype>
    {
        System.Threading.Tasks.Task<Reporttype?> GetByShortcodeAsync(string shortcode);
        System.Threading.Tasks.Task<List<Reporttype>> GetToEdit();

    }
}