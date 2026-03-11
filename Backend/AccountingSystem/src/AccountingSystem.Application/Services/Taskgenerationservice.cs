//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using AccountingSystem.Domain.Entities;
//using AccountingSystem.Domain.Enums;

//namespace AccountingSystem.Application.Services
//{
//    /// <summary>
//    /// שירות ליצירה אוטומטית של משימות
//    /// זה הלב של המערכת - יוצר משימות חודשיות/שנתיות/רבעוניות אוטומטית
//    /// </summary>
//    public class TaskGenerationService
//    {
//        private readonly ApplicationDbContext _context;
        
//        public TaskGenerationService(ApplicationDbContext context)
//        {
//            _context = context;
//        }
        
//        /// <summary>
//        /// יצירת משימות חודשיות לכל החברות הפעילות
//        /// קוראים לזה בתחילת כל חודש (Job אוטומטי או ידני)
//        /// </summary>
//        public async Task<int> CreateMonthlyTasks(int year, int month)
//        {
//            int tasksCreated = 0;
            
//            // שלב 1: מציאת כל התצורות החודשיות הפעילות
//            var monthlyConfigs = await _context.TaskTypeConfigurations
//                .Include(c => c.TaskType)
//                .Where(c => c.RecurrenceType == RecurrenceType.Monthly && c.IsActive)
//                .ToListAsync();
            
//            if (!monthlyConfigs.Any())
//            {
//                return 0; // אין הגדרות חודשיות
//            }
            
//            // שלב 2: מציאת כל החברות הפעילות
//            var activeCompanies = await _context.Companies
//                .Where(c => c.IsActive) // נניח שיש שדה IsActive
//                .ToListAsync();
            
//            // שלב 3: לכל חברה, לכל סוג משימה - בדוק אם צריך ליצור
//            foreach (var company in activeCompanies)
//            {
//                foreach (var config in monthlyConfigs)
//                {
//                    // בדיקה: האם החברה צריכה משימה זו?
//                    if (!await ShouldCreateTaskForCompany(company.Id, config.TaskTypeId))
//                    {
//                        continue; // דלג על חברה זו
//                    }
                    
//                    // בדיקה: האם כבר קיימת משימה לתקופה זו?
//                    var periodDate = new DateOnly(year, month, 1);
//                    var exists = await _context.CompanyTasks
//                        .AnyAsync(t => 
//                            t.Companyid == company.Id && 
//                            t.Tasktypeid == config.TaskTypeId && 
//                            t.Period == periodDate);
                    
//                    if (exists)
//                    {
//                        continue; // כבר יש משימה לחודש זה
//                    }
                    
//                    // יצירת המשימה
//                    var task = await CreateTaskFromConfiguration(
//                        company.Id, 
//                        config, 
//                        periodDate
//                    );
                    
//                    _context.CompanyTasks.Add(task);
//                    tasksCreated++;
//                }
//            }
            
//            await _context.SaveChangesAsync();
//            return tasksCreated;
//        }
        
//        /// <summary>
//        /// יצירת משימות רבעוניות
//        /// קוראים לזה בתחילת כל רבעון (ינואר, אפריל, יולי, אוקטובר)
//        /// </summary>
//        public async Task<int> CreateQuarterlyTasks(int year, int quarter)
//        {
//            // quarter = 1-4 (Q1, Q2, Q3, Q4)
//            int month = (quarter - 1) * 3 + 1; // Q1=1, Q2=4, Q3=7, Q4=10
            
//            var quarterlyConfigs = await _context.TaskTypeConfigurations
//                .Include(c => c.TaskType)
//                .Where(c => c.RecurrenceType == RecurrenceType.Quarterly && c.IsActive)
//                .ToListAsync();
            
//            return await CreateTasksForPeriod(year, month, quarterlyConfigs);
//        }
        
//        /// <summary>
//        /// יצירת משימות שנתיות
//        /// קוראים לזה בתחילת השנה או בסוף שנה קודמת
//        /// </summary>
//        public async Task<int> CreateYearlyTasks(int year)
//        {
//            var yearlyConfigs = await _context.TaskTypeConfigurations
//                .Include(c => c.TaskType)
//                .Where(c => c.RecurrenceType == RecurrenceType.Yearly && c.IsActive)
//                .ToListAsync();
            
//            return await CreateTasksForPeriod(year, 1, yearlyConfigs);
//        }
        
//        /// <summary>
//        /// יצירת משימה בודדת לחברה ספציפית
//        /// שימושי ליצירה ידנית או למשימות חד-פעמיות
//        /// </summary>
//        public async Task<CompanyTask> CreateSingleTask(
//            int companyId, 
//            int taskTypeId, 
//            DateOnly period,
//            DateOnly? customDueDate = null,
//            int? assignedWorkerId = null,
//            string? notes = null)
//        {
//            var config = await _context.TaskTypeConfigurations
//                .FirstOrDefaultAsync(c => c.TaskTypeId == taskTypeId && c.IsActive);
            
//            var task = new CompanyTask
//            {
//                Companyid = companyId,
//                Tasktypeid = taskTypeId,
//                Period = period,
//                Duedate = customDueDate ?? CalculateDueDate(period, config),
//                Status = TaskStatus1.Pending,
//                Assignedworkerid = assignedWorkerId,
//                Notes = notes,
//                Priority = TaskPriority.Normal,
//                Createdat = DateTime.UtcNow
//            };
            
//            // יצירת Checklist אוטומטית אם יש
//            await AttachChecklistItems(task, taskTypeId);
            
//            _context.CompanyTasks.Add(task);
//            await _context.SaveChangesAsync();
            
//            return task;
//        }
        
//        /// <summary>
//        /// יצירת המשימה הבאה אחרי שהשלמנו משימה נוכחית
//        /// עובד רק אם AutoCreateNext = true
//        /// </summary>
//        public async Task<CompanyTask?> CreateNextRecurringTask(int completedTaskId)
//        {
//            var completedTask = await _context.CompanyTasks
//                .Include(t => t.Tasktype)
//                .FirstOrDefaultAsync(t => t.Id == completedTaskId);
            
//            if (completedTask == null)
//                return null;
            
//            var config = await _context.TaskTypeConfigurations
//                .FirstOrDefaultAsync(c => 
//                    c.TaskTypeId == completedTask.Tasktypeid && 
//                    c.AutoCreateNext &&
//                    c.IsActive);
            
//            if (config == null)
//                return null; // לא מוגדר ליצור אוטומטית
            
//            // חישוב התקופה הבאה
//            var nextPeriod = CalculateNextPeriod(completedTask.Period, config.RecurrenceType);
            
//            // בדיקה שלא קיימת כבר
//            var exists = await _context.CompanyTasks
//                .AnyAsync(t => 
//                    t.Companyid == completedTask.Companyid && 
//                    t.Tasktypeid == completedTask.Tasktypeid && 
//                    t.Period == nextPeriod);
            
//            if (exists)
//                return null;
            
//            return await CreateSingleTask(
//                completedTask.Companyid,
//                completedTask.Tasktypeid,
//                nextPeriod,
//                assignedWorkerId: completedTask.Assignedworkerid
//            );
//        }
        
//        // ==========================================
//        // פונקציות עזר פרטיות
//        // ==========================================
        
//        /// <summary>
//        /// בדיקה האם צריך ליצור משימה לחברה זו
//        /// בודק את CompanyTaskTypeSettings
//        /// </summary>
//        private async Task<bool> ShouldCreateTaskForCompany(int companyId, int taskTypeId)
//        {
//            var settings = await _context.CompanyTaskTypeSettings
//                .FirstOrDefaultAsync(s => 
//                    s.CompanyId == companyId && 
//                    s.TaskTypeId == taskTypeId);
            
//            // אם אין הגדרות ספציפיות - תלוי ב-IsMandatory
//            if (settings == null)
//            {
//                var config = await _context.TaskTypeConfigurations
//                    .FirstOrDefaultAsync(c => c.TaskTypeId == taskTypeId);
//                return config?.IsMandatory ?? false;
//            }
            
//            // אם יש הגדרות ספציפיות - תלוי ב-IsActive שלהן
//            return settings.IsActive;
//        }
        
//        /// <summary>
//        /// יצירת משימה מתצורה
//        /// </summary>
//        private async Task<CompanyTask> CreateTaskFromConfiguration(
//            int companyId, 
//            TaskTypeConfiguration config, 
//            DateOnly period)
//        {
//            // קבלת הגדרות ספציפיות לחברה (אם יש)
//            var companySettings = await _context.CompanyTaskTypeSettings
//                .FirstOrDefaultAsync(s => 
//                    s.CompanyId == companyId && 
//                    s.TaskTypeId == config.TaskTypeId);
            
//            var task = new CompanyTask
//            {
//                Companyid = companyId,
//                Tasktypeid = config.TaskTypeId,
//                Period = period,
//                Duedate = CalculateDueDate(period, config, companySettings),
//                Status = TaskStatus1.Pending,
//                Assignedworkerid = companySettings?.DefaultAssignedWorkerId,
//                Notes = companySettings?.DefaultNotes,
//                Priority = companySettings?.CustomPriority ?? TaskPriority.Normal,
//                Createdat = DateTime.UtcNow
//            };
            
//            // צירוף Checklist אוטומטי
//            await AttachChecklistItems(task, config.TaskTypeId);
            
//            return task;
//        }
        
//        /// <summary>
//        /// חישוב תאריך יעד למשימה
//        /// </summary>
//        private DateOnly? CalculateDueDate(
//            DateOnly period, 
//            TaskTypeConfiguration? config,
//            CompanyTaskTypeSettings? companySettings = null)
//        {
//            if (config == null)
//                return null;
            
//            // אם יש override מהחברה - השתמש בו
//            if (companySettings?.CustomDueDayOfMonth != null)
//            {
//                return new DateOnly(period.Year, period.Month, companySettings.CustomDueDayOfMonth.Value);
//            }
            
//            // אם מוגדר יום בחודש
//            if (config.DueDayOfMonth.HasValue)
//            {
//                int day = Math.Min(config.DueDayOfMonth.Value, 
//                    DateTime.DaysInMonth(period.Year, period.Month));
//                return new DateOnly(period.Year, period.Month, day);
//            }
            
//            // אם מוגדר ימים אחרי סוף התקופה
//            if (config.DueDaysAfterPeriodEnd.HasValue)
//            {
//                var lastDayOfMonth = new DateOnly(period.Year, period.Month, 
//                    DateTime.DaysInMonth(period.Year, period.Month));
//                return lastDayOfMonth.AddDays(config.DueDaysAfterPeriodEnd.Value);
//            }
            
//            return null; // אין הגדרה
//        }
        
//        /// <summary>
//        /// חישוב התקופה הבאה לפי תדירות
//        /// </summary>
//        private DateOnly CalculateNextPeriod(DateOnly currentPeriod, RecurrenceType recurrence)
//        {
//            return recurrence switch
//            {
//                RecurrenceType.Monthly => currentPeriod.AddMonths(1),
//                RecurrenceType.BiMonthly => currentPeriod.AddMonths(2),
//                RecurrenceType.Quarterly => currentPeriod.AddMonths(3),
//                RecurrenceType.Yearly => currentPeriod.AddYears(1),
//                _ => currentPeriod
//            };
//        }
        
//        /// <summary>
//        /// צירוף פריטי Checklist למשימה
//        /// </summary>
//        private async Task AttachChecklistItems(CompanyTask task, int taskTypeId)
//        {
//            var template = await _context.TaskChecklistTemplates
//                .Include(t => t.Items)
//                .FirstOrDefaultAsync(t => 
//                    t.TaskTypeId == taskTypeId && 
//                    t.IsActive && 
//                    t.AutoCreateItems);
            
//            if (template == null || !template.Items.Any())
//                return;
            
//            foreach (var templateItem in template.Items.OrderBy(i => i.OrderIndex))
//            {
//                var checklistItem = new CompanyTaskChecklistItem
//                {
//                    CompanyTask = task,
//                    TemplateItemId = templateItem.Id,
//                    Title = templateItem.Title,
//                    Description = templateItem.Description,
//                    OrderIndex = templateItem.OrderIndex,
//                    IsCompleted = false,
//                    CreatedAt = DateTime.UtcNow
//                };
                
//                task.ChecklistItems.Add(checklistItem);
//            }
//        }
        
//        /// <summary>
//        /// פונקציה כללית ליצירת משימות לתקופה
//        /// </summary>
//        private async Task<int> CreateTasksForPeriod(
//            int year, 
//            int month, 
//            List<TaskTypeConfiguration> configs)
//        {
//            int tasksCreated = 0;
//            var activeCompanies = await _context.Companies
//                .Where(c => c.IsActive)
//                .ToListAsync();
            
//            var periodDate = new DateOnly(year, month, 1);
            
//            foreach (var company in activeCompanies)
//            {
//                foreach (var config in configs)
//                {
//                    if (!await ShouldCreateTaskForCompany(company.Id, config.TaskTypeId))
//                        continue;
                    
//                    var exists = await _context.CompanyTasks
//                        .AnyAsync(t => 
//                            t.Companyid == company.Id && 
//                            t.Tasktypeid == config.TaskTypeId && 
//                            t.Period == periodDate);
                    
//                    if (exists)
//                        continue;
                    
//                    var task = await CreateTaskFromConfiguration(company.Id, config, periodDate);
//                    _context.CompanyTasks.Add(task);
//                    tasksCreated++;
//                }
//            }
            
//            await _context.SaveChangesAsync();
//            return tasksCreated;
//        }
//    }
//}