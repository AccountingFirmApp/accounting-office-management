using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AccountingSystem.Domain.Interfaces.Repositories
{
   
    public interface IWorkerRepository : IGenericRepository<Worker>

    {
        System.Threading.Tasks.Task AddAsync(Worker worker);


        Task<IEnumerable<Worker>> GetAllByFirmIdAsync(int firmId, bool? isActive = true);
        //Task<IEnumerable<Worker>> GetAllByFirmIdAsync(int firmId);

        Task<Worker?> GetByIdAndFirmIdAsync(int id, int firmId);

       
        Task<Worker?> GetByEmailAsync(string email);
        Task<Worker?> GetByEmailAsync(string email, int firmId);

       
        System.Threading.Tasks.Task<Worker?> GetWorkerWithRoleAsync(int workerId);

       
        System.Threading.Tasks.Task<Worker?> GetWorkerWithCompaniesAsync(int workerId);

        
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetWorkersByFirmIdAsync(int firmId);

        
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(int companyId);

        
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetActiveWorkersAsync();

       
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetWorkersByRoleIdAsync(int roleId);

       
        System.Threading.Tasks.Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null);



    }
}