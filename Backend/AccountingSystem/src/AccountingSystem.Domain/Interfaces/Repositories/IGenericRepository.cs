using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
        public interface IGenericRepository<T> where T : class
    {
        // ========== קריאה ==========

       
        System.Threading.Tasks.Task<T?> GetByIdAsync(int id);

        
        System.Threading.Tasks.Task<IEnumerable<T>> GetAllAsync();

                System.Threading.Tasks.Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // ========== כתיבה ==========

        
        System.Threading.Tasks.Task UpdateAsync(T entity);

               System.Threading.Tasks.Task DeleteAsync(int id);

        // ========== בדיקות ==========

      
        System.Threading.Tasks.Task<bool> ExistsAsync(int id);

       
        System.Threading.Tasks.Task<int> CountAsync(Func<object, bool> value);
    }
}