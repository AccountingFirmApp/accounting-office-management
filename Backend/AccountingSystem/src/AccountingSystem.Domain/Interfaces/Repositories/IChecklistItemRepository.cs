using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
   
        public interface IChecklistItemRepository
        {
            Task<CompanyTaskChecklistItem?> GetByIdAsync(int id);
            void Update(CompanyTaskChecklistItem item);
            Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

            Task AddAsync(CompanyTaskChecklistItem item);
        }
    }

