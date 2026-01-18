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
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// תביא לי את כל הEntities
        /// דוגמה: תביא לי את כל החברות
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// תביא לי Entities לפי תנאי
        /// דוגמה: תביא לי את כל החברות שהסטטוס שלהן Active
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // ========== כתיבה ==========

        /// <summary>
        /// תוסיף Entity חדש
        /// דוגמה: תוסיף חברה חדשה למערכת
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// תעדכן Entity קיים
        /// דוגמה: תשנה את שם החברה
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// תמחק Entity
        /// דוגמה: תמחק חברה מהמערכת
        /// </summary>
        Task DeleteAsync(int id);

        // ========== בדיקות ==========

        /// <summary>
        /// האם Entity קיים?
        /// דוגמה: האם יש חברה עם ID = 12345?
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// כמה Entities יש?
        /// דוגמה: כמה חברות יש במערכת?
        /// </summary>
        Task<int> CountAsync(Func<object, bool> value);
    }
}