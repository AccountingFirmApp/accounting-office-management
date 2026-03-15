using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
   
        public interface ITaskTypeConfigurationRepository
        {
            Task<IEnumerable<TaskTypeConfiguration>> GetAllAsync();
            Task<TaskTypeConfiguration?> GetByTaskTypeIdAsync(int taskTypeId);
        }
    }
