////////////using AccountingSystem.Domain.Entities;
////////////using AccountingSystem.Domain.Interfaces.Repositories;
////////////using AccountingSystem.Infrastructure.Data;
////////////using System;
////////////using System.Collections.Generic;
////////////using System.Linq;
////////////using System.Linq.Expressions;
////////////using System.Text;
////////////using System.Threading.Tasks;

////////////namespace AccountingSystem.Infrastructure.Repositories
////////////{
////////////    public class ReportInstanceRepository : IReportInstanceRepository
////////////    {
////////////        private AccountingDbContext context;

////////////        public ReportInstanceRepository(AccountingDbContext context)
////////////        {
////////////            this.context = context;
////////////        }
////////////        public Task<Reportinstance> AddAsync(Reportinstance entity)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<int> CountAsync(Func<object, bool> value)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public System.Threading.Tasks.Task DeleteAsync(int id)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<bool> ExistsAsync(int id)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetAllAsync()
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<Reportinstance?> GetByIdAsync(int id)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
////////////        {
////////////            throw new NotImplementedException();
////////////        }

////////////        public System.Threading.Tasks.Task UpdateAsync(Reportinstance entity)
////////////        {
////////////            throw new NotImplementedException();
////////////        }
////////////    }
////////////}


//////////using AccountingSystem.Domain.Entities;
//////////using AccountingSystem.Domain.Interfaces;
//////////using AccountingSystem.Domain.Interfaces.Repositories;
//////////using AccountingSystem.Infrastructure.Data;
//////////using Microsoft.EntityFrameworkCore;
//////////using System.Linq.Expressions;

//////////namespace AccountingSystem.Infrastructure.Repositories
//////////{
//////////    public class ReportInstanceRepository : IReportInstanceRepository
//////////    {
//////////        private readonly AccountingDbContext _context;

//////////        public ReportInstanceRepository(AccountingDbContext context)
//////////        {
//////////            _context = context;
//////////        }

//////////        public async Task<Reportinstance?> GetByIdAsync(int id)
//////////        {
//////////            return await _context.Reportinstances
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Company)
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Reporttype)
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Frequency)
//////////                .FirstOrDefaultAsync(r => r.Id == id);
//////////        }

//////////        public async Task<List<Reportinstance>> GetAllAsync()
//////////        {
//////////            return await _context.Reportinstances
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Company)
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Reporttype)
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Frequency)
//////////                .ToListAsync();
//////////        }

//////////        public async Task<List<Reportinstance>> GetByCompanyIdAsync(int companyId)
//////////        {
//////////            return await _context.Reportinstances
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Company)
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Reporttype)
//////////                .Include(r => r.Config)
//////////                    .ThenInclude(c => c.Frequency)
//////////                .Where(r => r.Config.Companyid == companyId)
//////////                .ToListAsync();
//////////        }

//////////        public async Task AddAsync(Reportinstance reportInstance)
//////////        {
//////////            await _context.Reportinstances.AddAsync(reportInstance);
//////////        }

//////////        public void UpdateAsync(Reportinstance reportInstance)
//////////        {
//////////            _context.Reportinstances.Update(reportInstance);
//////////        }

//////////        public void DeleteAsync(Reportinstance reportInstance)
//////////        {
//////////            _context.Reportinstances.Remove(reportInstance);
//////////        }



//////////        public Task<int> CountAsync(Func<object, bool> value)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        //public System.Threading.Tasks.Task DeleteAsync(int id)
//////////        //{
//////////        //    throw new NotImplementedException();
//////////        //}

//////////        public Task<bool> ExistsAsync(int id)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        //public Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
//////////        //{
//////////        //    throw new NotImplementedException();
//////////        //}

//////////        //        public Task<IEnumerable<Reportinstance>> GetAllAsync()
//////////        //        {
//////////        //            throw new NotImplementedException();
//////////        //        }

//////////        //        public Task<Reportinstance?> GetByIdAsync(int id)
//////////        //        {
//////////        //            throw new NotImplementedException();
//////////        //        }

//////////        public Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        Task<IEnumerable<Reportinstance>> IGenericRepository<Reportinstance>.GetAllAsync()
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        Task<Reportinstance> IGenericRepository<Reportinstance>.AddAsync(Reportinstance entity)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        System.Threading.Tasks.Task IGenericRepository<Reportinstance>.UpdateAsync(Reportinstance entity)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        public System.Threading.Tasks.Task DeleteAsync(int id)
//////////        {
//////////            throw new NotImplementedException();
//////////        }

//////////        //        public System.Threading.Tasks.Task UpdateAsync(Reportinstance entity)
//////////        //        {
//////////        //            throw new NotImplementedException();
//////////        //        }
//////////    }
//////////}











////////using AccountingSystem.Domain.Entities;
////////using AccountingSystem.Domain.Interfaces.Repositories;
////////using AccountingSystem.Infrastructure.Data;
////////using Microsoft.EntityFrameworkCore;
////////using System;
////////using System.Collections.Generic;
////////using System.Linq;
////////using System.Threading.Tasks;

////////namespace AccountingSystem.Infrastructure.Repositories
////////{
////////    /// <summary>
////////    /// מימוש של פעולות Repository עבור דוחות
////////    /// </summary>
////////    //public class ReportInstanceRepository : GenericRepository<Reportinstance>, IReportInstanceRepository
////////    //{
////////    //    public ReportInstanceRepository(AccountingSystemDbContext context) : base(context)
////////    //    {

////////    //    }

////////    public class ReportInstanceRepository : IReportInstanceRepository
////////    {
////////        private AccountingDbContext _context;

////////        public ReportInstanceRepository(AccountingDbContext context)
////////        {
////////            this._context = context;
////////        }

////////        // ========== חיפושים לפי חברה ==========

////////        /// <summary>
////////        /// תביא לי את כל הדוחות של חברה
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
////////        {
////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Frequency)
////////                .Where(r => r.Config.Companyid == companyId)
////////                .OrderByDescending(r => r.Period)
////////                .ToListAsync();
////////        }

////////        /// <summary>
////////        /// תביא לי את כל הדוחות של הגדרה מסוימת
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
////////        {
////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Frequency)
////////                .Where(r => r.Configid == configId)
////////                .OrderByDescending(r => r.Period)
////////                .ToListAsync();
////////        }

////////        // ========== חיפושים לפי סטטוס ==========

////////        /// <summary>
////////        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
////////        {
////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Where(r => r.Status.ToString() == status)
////////                .OrderByDescending(r => r.Period)
////////                .ToListAsync();
////////        }

////////        /// <summary>
////////        /// תביא לי דוחות שממתינים (Pending)
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
////////        {
////////            return await GetReportsByStatusAsync("Pending");
////////        }

////////        // ========== חיפושים לפי תקופה ==========

////////        /// <summary>
////////        /// תביא לי דוחות של תקופה מסוימת
////////        /// דוגמה: כל הדוחות של 01/2024
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
////////        {
////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Where(r => r.Period.HasValue &&
////////                           r.Period.Value.Year == period.Year &&
////////                           r.Period.Value.Month == period.Month)
////////                .OrderBy(r => r.Config.Company.Companyname)
////////                .ToListAsync();
////////        }

////////        /// <summary>
////////        /// תביא לי דוחות בטווח תאריכים
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
////////        {
////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Where(r => r.Period.HasValue &&
////////                           r.Period.Value >= startDate &&
////////                           r.Period.Value <= endDate)
////////                .OrderByDescending(r => r.Reportperiod)
////////                .ToListAsync();
////////        }

////////        // ========== דוחות שדורשים תשומת לב! ==========

////////        /// <summary>
////////        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
////////        /// זה קריטי! צריך להתריע על דוחות באיחור
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
////////        {
////////            var today = DateTime.UtcNow.Date;

////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Where(r => r.Duedate.HasValue &&
////////                           r.Duedate.Value.Date < today &&
////////                           r.Currentstatus != "Submitted" &&
////////                           r.Currentstatus != "Approved")
////////                .OrderBy(r => r.Duedate)
////////                .ToListAsync();
////////        }

////////        /// <summary>
////////        /// תביא לי דוחות שצריך להגיש בעוד X ימים
////////        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
////////        /// </summary>
////////        public async Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
////////        {
////////            var today = DateTime.UtcNow.Date;
////////            var futureDate = today.AddDays(days);

////////            return await _context.Reportinstances
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Company)
////////                .Include(r => r.Config)
////////                    .ThenInclude(c => c.Reporttype)
////////                .Where(r => r.Duedate.HasValue &&
////////                           r.Duedate.Value.Date >= today &&
////////                           r.Duedate.Value.Date <= futureDate &&
////////                           r.Currentstatus != "Submitted" &&
////////                           r.Currentstatus != "Approved")
////////                .OrderBy(r => r.Duedate)
////////                .ToListAsync();
////////        }
////////    }
////////}




//////using AccountingSystem.Domain.Entities;
//////using AccountingSystem.Domain.Interfaces.Repositories;
//////using AccountingSystem.Infrastructure.Data;
//////using Microsoft.EntityFrameworkCore;
//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Threading.Tasks;

//////namespace AccountingSystem.Infrastructure.Repositories
//////{
//////    /// <summary>
//////    /// מימוש של פעולות Repository עבור דוחות
//////    /// </summary>
//////    //public class ReportInstanceRepository : GenericRepository<Reportinstance>, IReportInstanceRepository
//////    //{
//////    //    public ReportInstanceRepository(AccountingSystemDbContext _context) : base(_context)
//////    //    {
//////    //    }


//////    public class ReportInstanceRepository : IReportInstanceRepository
//////    {
//////        private readonly AccountingDbContext _context;

//////        public ReportInstanceRepository(AccountingDbContext context)
//////        {
//////            _context = context;
//////        }

//////        // ========== חיפושים לפי חברה ==========

//////        /// <summary>
//////        /// תביא לי את כל הדוחות של חברה
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
//////        {
//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Frequency)
//////                .Where(r => r.Config.Companyid == companyId)
//////                .OrderByDescending(r => r.Reportperiod)
//////                .ToListAsync();
//////        }

//////        /// <summary>
//////        /// תביא לי את כל הדוחות של הגדרה מסוימת
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
//////        {
//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Frequency)
//////                .Where(r => r.Configid == configId)
//////                .OrderByDescending(r => r.Reportperiod)
//////                .ToListAsync();
//////        }

//////        // ========== חיפושים לפי סטטוס ==========

//////        /// <summary>
//////        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
//////        {
//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Where(r => r.Currentstatus == status)
//////                .OrderByDescending(r => r.Reportperiod)
//////                .ToListAsync();
//////        }

//////        /// <summary>
//////        /// תביא לי דוחות שממתינים (Pending)
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
//////        {
//////            return await GetReportsByStatusAsync("Pending");
//////        }

//////        // ========== חיפושים לפי תקופה ==========

//////        /// <summary>
//////        /// תביא לי דוחות של תקופה מסוימת
//////        /// דוגמה: כל הדוחות של 01/2024
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
//////        {
//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Where(r => r.Reportperiod.HasValue &&
//////                           r.Reportperiod.Value.Year == period.Year &&
//////                           r.Reportperiod.Value.Month == period.Month)
//////                .OrderBy(r => r.Config.Company.Companyname)
//////                .ToListAsync();
//////        }

//////        /// <summary>
//////        /// תביא לי דוחות בטווח תאריכים
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
//////        {
//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Where(r => r.Reportperiod.HasValue &&
//////                           r.Reportperiod.Value >= startDate &&
//////                           r.Reportperiod.Value <= endDate)
//////                .OrderByDescending(r => r.Reportperiod)
//////                .ToListAsync();
//////        }

//////        // ========== דוחות שדורשים תשומת לב! ==========

//////        /// <summary>
//////        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
//////        /// זה קריטי! צריך להתריע על דוחות באיחור
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
//////        {
//////            var today = DateTime.UtcNow.Date;

//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Where(r => r.Duedate.HasValue &&
//////                           r.Duedate.Value.Date < today &&
//////                           r.Currentstatus != "Submitted" &&
//////                           r.Currentstatus != "Approved")
//////                .OrderBy(r => r.Duedate)
//////                .ToListAsync();
//////        }

//////        /// <summary>
//////        /// תביא לי דוחות שצריך להגיש בעוד X ימים
//////        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
//////        /// </summary>
//////        public async Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
//////        {
//////            var today = DateTime.UtcNow.Date;
//////            var futureDate = today.AddDays(days);

//////            return await _context.Reportinstances
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Company)
//////                .Include(r => r.Config)
//////                    .ThenInclude(c => c.Reporttype)
//////                .Where(r => r.Duedate.HasValue &&
//////                           r.Duedate.Value.Date >= today &&
//////                           r.Duedate.Value.Date <= futureDate &&
//////                           r.Currentstatus != "Submitted" &&
//////                           r.Currentstatus != "Approved")
//////                .OrderBy(r => r.Duedate)
//////                .ToListAsync();
//////        }
//////    }
//////}




////using AccountingSystem.Domain.Entities;
////using AccountingSystem.Domain.Interfaces.Repositories;
////using AccountingSystem.Infrastructure.Data;
////using Microsoft.EntityFrameworkCore;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Linq.Expressions;
////using System.Threading.Tasks;

////namespace AccountingSystem.Infrastructure.Repositories
////{
////    /// <summary>
////    /// מימוש של פעולות Repository עבור דוחות
////    /// </summary>
////    public class ReportInstanceRepository : IReportInstanceRepository
////    {
////        private AccountingDbContext _context;

////        public ReportInstanceRepository(AccountingDbContext context)
////        {
////            this._context = context;
////        }
////        // ========== חיפושים לפי חברה ==========

////        /// <summary>
////        /// תביא לי את כל הדוחות של חברה
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
////        {
////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Frequency)
////                .Where(r => r.Config.Companyid == companyId)
////                .OrderByDescending(r => r.Period)
////                .ToListAsync();
////        }

////        /// <summary>
////        /// תביא לי את כל הדוחות של הגדרה מסוימת
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
////        {
////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Frequency)
////                .Where(r => r.Configid == configId)
////                .OrderByDescending(r => r.Period)
////                .ToListAsync();
////        }

////        // ========== חיפושים לפי סטטוס ==========

////        /// <summary>
////        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
////        {
////            // המרה מ-string ל-Enum
////            if (Enum.TryParse<AccountingSystem.Domain.Enums.ReportStatus>(status, out var reportStatus))
////            {
////                return await _context.Reportinstances
////                    .Include(r => r.Config)
////                        .ThenInclude(c => c.Company)
////                    .Include(r => r.Config)
////                        .ThenInclude(c => c.Reporttype)
////                    .Where(r => r.Status == reportStatus)
////                    .OrderByDescending(r => r.Period)
////                    .ToListAsync();
////            }

////            return new List<Reportinstance>();
////        }

////        /// <summary>
////        /// תביא לי דוחות שממתינים (Pending)
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
////        {
////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Where(r => r.Status == AccountingSystem.Domain.Enums.ReportStatus.Pending)
////                .OrderByDescending(r => r.Period)
////                .ToListAsync();
////        }

////        // ========== חיפושים לפי תקופה ==========

////        /// <summary>
////        /// תביא לי דוחות של תקופה מסוימת
////        /// דוגמה: כל הדוחות של 01/2024
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
////        {
////            var dateOnly = DateOnly.FromDateTime(period);
////            var year = dateOnly.Year;
////            var month = dateOnly.Month;

////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Where(r => r.Period.Year == year && r.Period.Month == month)
////                .OrderBy(r => r.Config.Company.Name)
////                .ToListAsync();
////        }

////        /// <summary>
////        /// תביא לי דוחות בטווח תאריכים
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
////        {
////            var startDateOnly = DateOnly.FromDateTime(startDate);
////            var endDateOnly = DateOnly.FromDateTime(endDate);

////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Where(r => r.Period >= startDateOnly && r.Period <= endDateOnly)
////                .OrderByDescending(r => r.Period)
////                .ToListAsync();
////        }

////        // ========== דוחות שדורשים תשומת לב! ==========

////        /// <summary>
////        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
////        /// זה קריטי! צריך להתריע על דוחות באיחור
////        /// הערה: במודל שלך אין Duedate, אז מחשב לפי Period + תדירות
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
////        {
////            var today = DateOnly.FromDateTime(DateTime.UtcNow);

////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Frequency)
////                .Where(r => r.Period < today &&
////                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
////                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
////                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
////                .OrderBy(r => r.Period)
////                .ToListAsync();
////        }

////        /// <summary>
////        /// תביא לי דוחות שצריך להגיש בעוד X ימים
////        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
////        /// </summary>
////        public async Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
////        {
////            var today = DateOnly.FromDateTime(DateTime.UtcNow);
////            var futureDate = today.AddDays(days);

////            return await _context.Reportinstances
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Company)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Reporttype)
////                .Include(r => r.Config)
////                    .ThenInclude(c => c.Frequency)
////                .Where(r => r.Period >= today &&
////                           r.Period <= futureDate &&
////                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
////                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
////                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
////                .OrderBy(r => r.Period)
////                .ToListAsync();
////        }

////        public Task<Reportinstance?> GetByIdAsync(int id)
////        {
////            throw new NotImplementedException();
////        }

////        public Task<IEnumerable<Reportinstance>> GetAllAsync()
////        {
////            throw new NotImplementedException();
////        }

////        public Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
////        {
////            throw new NotImplementedException();
////        }

////        public Task<Reportinstance> AddAsync(Reportinstance entity)
////        {
////            throw new NotImplementedException();
////        }

////        public System.Threading.Tasks.Task UpdateAsync(Reportinstance entity)
////        {
////            throw new NotImplementedException();
////        }

////        public System.Threading.Tasks.Task DeleteAsync(int id)
////        {
////            throw new NotImplementedException();
////        }

////        public Task<bool> ExistsAsync(int id)
////        {
////            throw new NotImplementedException();
////        }

////        public Task<int> CountAsync(Func<object, bool> value)
////        {
////            throw new NotImplementedException();
////        }
////    }
////}


//using AccountingSystem.Domain.Entities;
//using AccountingSystem.Domain.Interfaces.Repositories;
//using AccountingSystem.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace AccountingSystem.Infrastructure.Repositories
//{
//    /// <summary>
//    /// מימוש של פעולות Repository עבור דוחות
//    /// </summary>
//    public class ReportInstanceRepository : IReportInstanceRepository
//    {
//        private readonly AccountingDbContext _context;

//        public ReportInstanceRepository(AccountingDbContext context)
//        {
//            _context = context;
//        }

//        // ========== פונקציות בסיסיות מ-IGenericRepository ==========

//        /// <summary>
//        /// תביא לי דוח לפי ID
//        /// </summary>
//        public async Task<Reportinstance?> GetByIdAsync(int id)
//        {
//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .FirstOrDefaultAsync(r => r.Id == id);
//        }

//        /// <summary>
//        /// תביא לי את כל הדוחות
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetAllAsync()
//        {
//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .OrderByDescending(r => r.Period)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// תביא לי דוחות לפי תנאי
//        /// דוגמה: FindAsync(r => r.Status == ReportStatus.Pending)
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
//        {
//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .Where(predicate)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// תוסיף דוח חדש
//        /// </summary>
//        public async Task<Reportinstance> AddAsync(Reportinstance entity)
//        {
//            await _context.Reportinstances.AddAsync(entity);
//            return entity;
//        }

//        /// <summary>
//        /// תעדכן דוח קיים
//        /// </summary>
//        public async Task UpdateAsync(Reportinstance entity)
//        {
//            _context.Reportinstances.Update(entity);
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// תמחק דוח לפי ID
//        /// </summary>
//        public async Task DeleteAsync(int id)
//        {
//            var entity = await _context.Reportinstances.FindAsync(id);
//            if (entity != null)
//            {
//                _context.Reportinstances.Remove(entity);
//            }
//        }

//        /// <summary>
//        /// האם דוח קיים?
//        /// </summary>
//        public async Task<bool> ExistsAsync(int id)
//        {
//            return await _context.Reportinstances.AnyAsync(r => r.Id == id);
//        }

//        /// <summary>
//        /// כמה דוחות יש?
//        /// </summary>
//        public async Task<int> CountAsync(Func<object, bool> value)
//        {
//            return await _context.Reportinstances.CountAsync();
//        }

//        // ========== חיפושים לפי חברה ==========

//        /// <summary>
//        /// תביא לי את כל הדוחות של חברה
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
//        {
//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .Where(r => r.Config.Companyid == companyId)
//                .OrderByDescending(r => r.Period)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// תביא לי את כל הדוחות של הגדרה מסוימת
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
//        {
//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .Where(r => r.Configid == configId)
//                .OrderByDescending(r => r.Period)
//                .ToListAsync();
//        }

//        // ========== חיפושים לפי סטטוס ==========

//        /// <summary>
//        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
//        {
//            // המרה מ-string ל-Enum
//            if (Enum.TryParse<AccountingSystem.Domain.Enums.ReportStatus>(status, out var reportStatus))
//            {
//                return await _context.Reportinstances
//                    .Include(r => r.Config)
//                        .ThenInclude(c => c.Company)
//                    .Include(r => r.Config)
//                        .ThenInclude(c => c.Reporttype)
//                    .Where(r => r.Status == reportStatus)
//                    .OrderByDescending(r => r.Period)
//                    .ToListAsync();
//            }

//            return new List<Reportinstance>();
//        }

//        /// <summary>
//        /// תביא לי דוחות שממתינים (Pending)
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
//        {
//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Where(r => r.Status == AccountingSystem.Domain.Enums.ReportStatus.Pending)
//                .OrderByDescending(r => r.Period)
//                .ToListAsync();
//        }

//        // ========== חיפושים לפי תקופה ==========

//        /// <summary>
//        /// תביא לי דוחות של תקופה מסוימת
//        /// דוגמה: כל הדוחות של 01/2024
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
//        {
//            var dateOnly = DateOnly.FromDateTime(period);
//            var year = dateOnly.Year;
//            var month = dateOnly.Month;

//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Where(r => r.Period.Year == year && r.Period.Month == month)
//                .OrderBy(r => r.Config.Company.Name)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// תביא לי דוחות בטווח תאריכים
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
//        {
//            var startDateOnly = DateOnly.FromDateTime(startDate);
//            var endDateOnly = DateOnly.FromDateTime(endDate);

//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Where(r => r.Period >= startDateOnly && r.Period <= endDateOnly)
//                .OrderByDescending(r => r.Period)
//                .ToListAsync();
//        }

//        // ========== דוחות שדורשים תשומת לב! ==========

//        /// <summary>
//        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
//        /// זה קריטי! צריך להתריע על דוחות באיחור
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
//        {
//            var today = DateOnly.FromDateTime(DateTime.UtcNow);

//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .Where(r => r.Period < today &&
//                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
//                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
//                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
//                .OrderBy(r => r.Period)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// תביא לי דוחות שצריך להגיש בעוד X ימים
//        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
//        /// </summary>
//        public async Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
//        {
//            var today = DateOnly.FromDateTime(DateTime.UtcNow);
//            var futureDate = today.AddDays(days);

//            return await _context.Reportinstances
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Company)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Reporttype)
//                .Include(r => r.Config)
//                    .ThenInclude(c => c.Frequency)
//                .Where(r => r.Period >= today &&
//                           r.Period <= futureDate &&
//                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
//                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
//                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
//                .OrderBy(r => r.Period)
//                .ToListAsync();
//        }
//    }
//}



using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Task = System.Threading.Tasks.Task;

namespace AccountingSystem.Infrastructure.Repositories
{
    /// <summary>
    /// מימוש של פעולות Repository עבור דוחות
    /// </summary>
    public class ReportInstanceRepository : IReportInstanceRepository
    {
        private readonly AccountingDbContext _context;

        public ReportInstanceRepository(AccountingDbContext context)
        {
            _context = context;
        }

        // ========== פונקציות בסיסיות מ-IGenericRepository ==========

        /// <summary>
        /// תביא לי דוח לפי ID
        /// </summary>
        public async Task<Reportinstance?> GetByIdAsync(int id)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// תביא לי את כל הדוחות
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetAllAsync()
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי דוחות לפי תנאי
        /// דוגמה: FindAsync(r => r.Status == ReportStatus.Pending)
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(predicate)
                .ToListAsync();
        }

        /// <summary>
        /// תוסיף דוח חדש
        /// </summary>
        public async Task<Reportinstance> AddAsync(Reportinstance entity)
        {
            await _context.Reportinstances.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// תעדכן דוח קיים
        /// </summary>
        public Task UpdateAsync(Reportinstance entity)
        {
            _context.Reportinstances.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// תמחק דוח לפי ID
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Reportinstances.FindAsync(id);
            if (entity != null)
            {
                _context.Reportinstances.Remove(entity);
            }
        }

        /// <summary>
        /// האם דוח קיים?
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Reportinstances.AnyAsync(r => r.Id == id);
        }

        /// <summary>
        /// כמה דוחות יש?
        /// </summary>
        public async Task<int> CountAsync(Func<object, bool> value)
        {
            return await _context.Reportinstances.CountAsync();
        }

        // ========== חיפושים לפי חברה ==========

        /// <summary>
        /// תביא לי את כל הדוחות של חברה
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Config.Companyid == companyId)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי את כל הדוחות של הגדרה מסוימת
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Configid == configId)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        // ========== חיפושים לפי סטטוס ==========

        /// <summary>
        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
        {
            // המרה מ-string ל-Enum
            if (Enum.TryParse<AccountingSystem.Domain.Enums.ReportStatus>(status, out var reportStatus))
            {
                return await _context.Reportinstances
                    .Include(r => r.Config)
                        .ThenInclude(c => c.Company)
                    .Include(r => r.Config)
                        .ThenInclude(c => c.Reporttype)
                    .Where(r => r.Status == reportStatus)
                    .OrderByDescending(r => r.Period)
                    .ToListAsync();
            }

            return new List<Reportinstance>();
        }

        /// <summary>
        /// תביא לי דוחות שממתינים (Pending)
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Where(r => r.Status == AccountingSystem.Domain.Enums.ReportStatus.Pending)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        // ========== חיפושים לפי תקופה ==========

        /// <summary>
        /// תביא לי דוחות של תקופה מסוימת
        /// דוגמה: כל הדוחות של 01/2024
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
        {
            var dateOnly = DateOnly.FromDateTime(period);
            var year = dateOnly.Year;
            var month = dateOnly.Month;

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Where(r => r.Period.Year == year && r.Period.Month == month)
                .OrderBy(r => r.Config.Company.Name)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי דוחות בטווח תאריכים
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Where(r => r.Period >= startDateOnly && r.Period <= endDateOnly)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        // ========== דוחות שדורשים תשומת לב! ==========

        /// <summary>
        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
        /// זה קריטי! צריך להתריע על דוחות באיחור
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Period < today &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
                .OrderBy(r => r.Period)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי דוחות שצריך להגיש בעוד X ימים
        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var futureDate = today.AddDays(days);

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Period >= today &&
                           r.Period <= futureDate &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
                .OrderBy(r => r.Period)
                .ToListAsync();
        }

        Task IGenericRepository<Reportinstance>.AddAsync(Reportinstance entity)
        {
            throw new NotImplementedException();
        }
    }
}