using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    /// <summary>
    /// ממשק בסיסי לכל הRepositories
    /// מגדיר את כל הפעולות הסטנדרטיות: CRUD + חיפוש
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        // ========== קריאה ==========

        /// <summary>
        /// תביא לי Entity לפי ID
        /// דוגמה: תביא לי את החברה עם ID = 12345
        /// </summary>
        System.Threading.Tasks.Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// תביא לי את כל הEntities
        /// דוגמה: תביא לי את כל החברות
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// תביא לי Entities לפי תנאי
        /// דוגמה: תביא לי את כל החברות שהסטטוס שלהן Active
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // ========== כתיבה ==========

        /// <summary>
        /// תוסיף Entity חדש
        /// דוגמה: תוסיף חברה חדשה למערכת
        /// </summary>
        //System.Threading.Tasks.Task AddAsync(T entity);

        /// <summary>
        /// תעדכן Entity קיים
        /// דוגמה: תשנה את שם החברה
        /// </summary>
        System.Threading.Tasks.Task UpdateAsync(T entity);

        /// <summary>
        /// תמחק Entity
        /// דוגמה: תמחק חברה מהמערכת
        /// </summary>
        System.Threading.Tasks.Task DeleteAsync(int id);

        // ========== בדיקות ==========

        /// <summary>
        /// האם Entity קיים?
        /// דוגמה: האם יש חברה עם ID = 12345?
        /// </summary>
        System.Threading.Tasks.Task<bool> ExistsAsync(int id);

        /// <summary>
        /// כמה Entities יש?
        /// דוגמה: כמה חברות יש במערכת?
        /// </summary>
        System.Threading.Tasks.Task<int> CountAsync(Func<object, bool> value);
    }
}