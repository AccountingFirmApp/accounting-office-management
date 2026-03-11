using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore; // חשוב מאוד עבור ToListAsync
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class TaskTypeRepository : ITaskTypeRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<Tasktype> _dbSet;

        public TaskTypeRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.Tasktypes; // ודאי שזה השם ב-DbContext שלך
        }

        // הפונקציה הקריטית ביותר עבור המטריצה
        public async Task<IEnumerable<Tasktype>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // מימוש שאר הפונקציות כדי שהקומפיילר לא יצעק
        public async Task<Tasktype?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Tasktype> AddAsync(Tasktype entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Tasktype entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tasktype>> FindAsync(Expression<Func<Tasktype, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<int> CountAsync(Func<object, bool> value)
        {
            return await _dbSet.CountAsync();
        }

        public async Task<IEnumerable<Tasktype>> GetByCategoryAsync(string category)
        {
            // אם אין לך עמודת קטגוריה ב-DB, אפשר להחזיר רשימה ריקה בינתיים
            return await _dbSet.ToListAsync();
        }
    }
}