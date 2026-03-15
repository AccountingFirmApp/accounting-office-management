
    using AccountingSystem.Domain.Entities;
    using AccountingSystem.Domain.Interfaces.Repositories;
    using AccountingSystem.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace AccountingSystem.Infrastructure.Repositories
    {
        public class TaskTypeConfigurationRepository : ITaskTypeConfigurationRepository
        {
            private readonly AccountingDbContext _context;

            public TaskTypeConfigurationRepository(AccountingDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<TaskTypeConfiguration>> GetAllAsync()
            {
                // שולף את כל הגדרות הבסיס מהטבלה
                return await _context.TaskTypeConfiguration.ToListAsync();
            }

            public async Task<TaskTypeConfiguration?> GetByTaskTypeIdAsync(int taskTypeId)
            {
                return await _context.TaskTypeConfiguration
                    .FirstOrDefaultAsync(t => t.TaskTypeId == taskTypeId);
            }
        }
    }
