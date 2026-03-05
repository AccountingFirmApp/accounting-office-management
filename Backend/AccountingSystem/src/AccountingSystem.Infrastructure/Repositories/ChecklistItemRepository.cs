using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AccountingSystem.Infrastructure.Repositories
{
    public class ChecklistItemRepository : IChecklistItemRepository
    {
        private readonly AccountingDbContext _context;

        public ChecklistItemRepository(AccountingDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyTaskChecklistItem?> GetByIdAsync(int id)
        {
            return await _context.CompanyTaskChecklistItems
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public void Update(CompanyTaskChecklistItem item)
        {
            _context.CompanyTaskChecklistItems.Update(item);
        }

        public async Task AddAsync(CompanyTaskChecklistItem item)
        {
            await _context.CompanyTaskChecklistItems.AddAsync(item);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
