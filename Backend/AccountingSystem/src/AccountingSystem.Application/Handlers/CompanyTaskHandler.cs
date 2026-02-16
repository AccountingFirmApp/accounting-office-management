
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Entities;

using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.Queries.Tasks;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Handlers.Tasks
{

    // ==========================================
    // HANDLER: יצירת משימה בודדת
    // ==========================================

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResult>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public CreateTaskCommandHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

    
        public async Task<CreateTaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (await _taskRepository.TaskExistsAsync(request.CompanyId, request.TaskTypeId, request.Period))
            {
                return new CreateTaskResult { Success = false, Message = "משימה זו כבר קיימת עבור התקופה הזו" };
            }

            var task = new CompanyTask
            {
                Companyid = request.CompanyId,
                Tasktypeid = request.TaskTypeId,
                Period = request.Period,
                Duedate = request.CustomDueDate,
                Status = TaskStatus1.Pending,
                Priority = TaskPriority.Normal,
                Assignedworkerid = request.AssignedWorkerId,
                Notes = request.Notes,
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow
            };

            // שלב א': שמירה ראשונית ליצירת מזהה (ID)
            await _taskRepository.AddAsync(task);
            // כאן ה-ID של task מתעדכן אוטומטית מה-DB

            // שלב ב': הוספת הצ'קליסט כשיש כבר ID
            await AttachChecklistAsync(task, request.TaskTypeId);

            // שלב ג': שמירת הפריטים
            await _taskRepository.UpdateAsync(task);

            return new CreateTaskResult
            {
                Success = true,
                TaskId = task.Id,
                Message = "המשימה נוצרה בהצלחה"
            };
        }
        private async Task AttachChecklistAsync(CompanyTask task, int taskTypeId)
        {
            var template = await _taskRepository.GetActiveChecklistTemplateAsync(taskTypeId);

            if (template == null || template.Items == null || !template.Items.Any())
                return;

            // וודאי שהרשימה מאותחלת כדי למנוע Null Reference
            task.ChecklistItems ??= new List<CompanyTaskChecklistItem>();

            foreach (var item in template.Items.OrderBy(i => i.OrderIndex))
            {
                var newItem = new CompanyTaskChecklistItem
                {
                    // התיקון: אנחנו לא מסתמכים רק על ה-Add, אלא מוודאים ש-EF יודע למי הוא שייך
                    CompanyTaskId = task.Id,
                    Title = item.Title,
                    Description = item.Description,
                    OrderIndex = item.OrderIndex,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                task.ChecklistItems.Add(newItem);
            }

            // בגלל שזה קורה ב-Job, לפעמים צריך להגיד ל-EF לעקוב אחרי הפריטים החדשים במפורש
            // אם יש לך גישה ל-_context כאן, אפשר להוסיף אותם ישירות
        }
    }
        // ==========================================
        // HANDLER: יצירת משימות חודשיות
        // ==========================================

        public class GenerateMonthlyTasksCommandHandler
            : IRequestHandler<GenerateMonthlyTasksCommand, GenerateTasksResult>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public GenerateMonthlyTasksCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

        public async Task<GenerateTasksResult> Handle(
 GenerateMonthlyTasksCommand request,
 CancellationToken cancellationToken)
        {
            // 1. טעינת הגדרות חודשיות פעילות
            var monthlyConfigs = await _taskRepository
                .GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.Monthly);

            if (!monthlyConfigs.Any())
            {
                return new GenerateTasksResult
                {
                    Success = true,
                    TasksCreated = 0,
                    Message = "לא נמצאו תצורות חודשיות פעילות"
                };
            }

            // 2. טעינת חברות פעילות והגדרת תקופה
            var activeCompanies = await _taskRepository.GetActiveCompaniesAsync();
            var periodDate = new DateOnly(request.Year, request.Month, 1);
            int createdCount = 0;

            // 3. מעבר על כל חברה וכל הגדרה
            foreach (var company in activeCompanies)
            {
                //foreach (var config in monthlyConfigs)
                //{
                //    // בדיקה האם צריך ליצור את המשימה הספציפית הזו
                //    if (!await ShouldCreateTaskAsync(company.Id, config))
                //        continue;

                //    // מניעת כפילויות
                //    if (await _taskRepository.TaskExistsAsync(company.Id, config.TaskTypeId, periodDate))
                //        continue;

                //    // --- תהליך היצירה והשמירה בשלבים ---

                //    // א. יצירת אובייקט המשימה (וודאי שאין קריאה לצ'קליסט בתוך הפונקציה הזו)
                //    var task = await CreateTaskFromConfigAsync(company.Id, config, periodDate);

                //    // ב. שמירה ראשונית בבסיס הנתונים כדי לקבל ID
                //    await _taskRepository.AddAsync(task);

                //    // ג. הוספת הצ'קליסט למשימה (עכשיו כשיש לה ID)
                //    await AttachChecklistAsync(task, config.TaskTypeId);

                //    // ד. עדכון סופי של המשימה עם הצ'קליסט ב-DB
                //    await _taskRepository.UpdateAsync(task);

                //    createdCount++;
                //}
                foreach (var config in monthlyConfigs)
                {
                    if (!await ShouldCreateTaskAsync(company.Id, config)) continue;
                    if (await _taskRepository.TaskExistsAsync(company.Id, config.TaskTypeId, periodDate)) continue;

                    // 1. יצירת האובייקט
                    var task = await CreateTaskFromConfigAsync(company.Id, config, periodDate);

                    // 2. הוספת הצ'קליסט לאובייקט בזיכרון (הוא עדיין לא ב-DB)
                    // הערה: חשוב ש-AttachChecklistAsync רק יוסיף לרשימה task.ChecklistItems
                    await AttachChecklistAsync(task, config.TaskTypeId);

                    // 3. שמירה אחת ויחידה להכל! 
                    // EF יזהה שיש כאן משימה חדשה עם פריטים חדשים ויעשה את הקישור אוטומטית.
                    await _taskRepository.AddAsync(task);

                    createdCount++;
                }
            }

            return new GenerateTasksResult
            {
                Success = true,
                TasksCreated = createdCount,
                Message = $"נוצרו {createdCount} משימות חודשיות ל-{request.Month:D2}/{request.Year}"
            };
        }

        private async Task<bool> ShouldCreateTaskAsync(
                int companyId,
                TaskTypeConfiguration config)
            {
                var settings = await _taskRepository.GetCompanySettingsAsync(
                    companyId,
                    config.TaskTypeId);

                if (settings == null)
                    return config.IsMandatory;

                return settings.IsActive;
            }

            private async Task<CompanyTask> CreateTaskFromConfigAsync(
                int companyId,
                TaskTypeConfiguration config,
                DateOnly period)
            {
                var companySettings = await _taskRepository.GetCompanySettingsAsync(
                    companyId,
                    config.TaskTypeId);

                var task = new CompanyTask
                {
                    Companyid = companyId,
                    Tasktypeid = config.TaskTypeId,
                    Period = period,
                    Duedate = CalculateDueDate(period, config, companySettings),
                    Status = TaskStatus1.Pending,
                    Assignedworkerid = companySettings?.DefaultAssignedWorkerId,
                    Notes = companySettings?.DefaultNotes,
                    Priority = companySettings?.CustomPriority ?? TaskPriority.Normal,
                    Createdat = DateTime.UtcNow,
                    Updatedat = DateTime.UtcNow
                };

                await AttachChecklistAsync(task, config.TaskTypeId);

                return task;
            }

            private DateOnly? CalculateDueDate(
                DateOnly period,
                TaskTypeConfiguration? config,
                CompanyTaskTypeSettings? companySettings = null)
            {
                if (config == null)
                    return null;

                if (companySettings?.CustomDueDayOfMonth != null)
                {
                    int day = Math.Min(companySettings.CustomDueDayOfMonth.Value,
                        DateTime.DaysInMonth(period.Year, period.Month));
                    return new DateOnly(period.Year, period.Month, day);
                }

                if (config.DueDayOfMonth.HasValue)
                {
                    int day = Math.Min(config.DueDayOfMonth.Value,
                        DateTime.DaysInMonth(period.Year, period.Month));
                    return new DateOnly(period.Year, period.Month, day);
                }

                if (config.DueDaysAfterPeriodEnd.HasValue)
                {
                    var lastDay = new DateOnly(period.Year, period.Month,
                        DateTime.DaysInMonth(period.Year, period.Month));
                    return lastDay.AddDays(config.DueDaysAfterPeriodEnd.Value);
                }

                return null;
            }

        private async Task AttachChecklistAsync(CompanyTask task, int taskTypeId)
        {
            Console.WriteLine($"[DEBUG] Looking for template for TaskTypeId: {taskTypeId}");

            var template = await _taskRepository.GetActiveChecklistTemplateAsync(taskTypeId);

            if (template == null)
            {
                Console.WriteLine($"[DEBUG] !!! No active template found for TaskTypeId: {taskTypeId}");
                return;
            }

            if (template.Items == null || !template.Items.Any())
            {
                Console.WriteLine($"[DEBUG] !!! Found template '{template.Name}', but it has 0 items!");
                return;
            }

            Console.WriteLine($"[DEBUG] Successfully found {template.Items.Count} items. Attaching to task...");

            task.ChecklistItems ??= new List<CompanyTaskChecklistItem>();

            foreach (var item in template.Items)
            {
                task.ChecklistItems.Add(new CompanyTaskChecklistItem
                {
                    Title = item.Title,
                    Description = item.Description,
                    OrderIndex = item.OrderIndex,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                    CompanyTask = task
                });
            }
        }
    }

        // ==========================================
        // HANDLER: יצירת משימות רבעוניות
        // ==========================================

        public class GenerateQuarterlyTasksCommandHandler
            : IRequestHandler<GenerateQuarterlyTasksCommand, GenerateTasksResult>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public GenerateQuarterlyTasksCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<GenerateTasksResult> Handle(
                GenerateQuarterlyTasksCommand request,
                CancellationToken cancellationToken)
            {
                var quarterlyConfigs = await _taskRepository
                    .GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.Quarterly);

                if (!quarterlyConfigs.Any())
                {
                    return new GenerateTasksResult
                    {
                        Success = true,
                        TasksCreated = 0,
                        Message = "לא נמצאו תצורות רבעוניות פעילות"
                    };
                }

                var activeCompanies = await _taskRepository.GetActiveCompaniesAsync();

                // חישוב החודש הראשון של הרבעון
                int firstMonthOfQuarter = (request.Quarter - 1) * 3 + 1;
                var periodDate = new DateOnly(request.Year, firstMonthOfQuarter, 1);

                var tasksToCreate = new List<CompanyTask>();

                foreach (var company in activeCompanies)
                {
                    foreach (var config in quarterlyConfigs)
                    {
                        var settings = await _taskRepository.GetCompanySettingsAsync(
                            company.Id,
                            config.TaskTypeId);

                        if (settings == null && !config.IsMandatory)
                            continue;

                        if (settings != null && !settings.IsActive)
                            continue;

                        if (await _taskRepository.TaskExistsAsync(
                            company.Id,
                            config.TaskTypeId,
                            periodDate))
                            continue;

                        var task = new CompanyTask
                        {
                            Companyid = company.Id,
                            Tasktypeid = config.TaskTypeId,
                            Period = periodDate,
                            Duedate = CalculateQuarterlyDueDate(request.Year, request.Quarter, config, settings),
                            Status = TaskStatus1.Pending,
                            Priority = settings?.CustomPriority ?? TaskPriority.Normal,
                            Assignedworkerid = settings?.DefaultAssignedWorkerId,
                            Notes = settings?.DefaultNotes,
                            Createdat = DateTime.UtcNow,
                            Updatedat = DateTime.UtcNow
                        };

                        await AttachChecklistAsync(task, config.TaskTypeId);
                        tasksToCreate.Add(task);
                    }
                }

                int created = 0;
                if (tasksToCreate.Any())
                {
                    created = await _taskRepository.CreateTasksAsync(tasksToCreate);
                }

                return new GenerateTasksResult
                {
                    Success = true,
                    TasksCreated = created,
                    Message = $"נוצרו {created} משימות רבעוניות לרבעון {request.Quarter}/{request.Year}"
                };
            }

            private DateOnly? CalculateQuarterlyDueDate(
                int year,
                int quarter,
                TaskTypeConfiguration config,
                CompanyTaskTypeSettings? settings)
            {
                // חודש אחרון של הרבעון
                int lastMonthOfQuarter = quarter * 3;

                if (settings?.CustomDueDayOfMonth != null)
                {
                    int day = Math.Min(settings.CustomDueDayOfMonth.Value,
                        DateTime.DaysInMonth(year, lastMonthOfQuarter));
                    return new DateOnly(year, lastMonthOfQuarter, day);
                }

                if (config.DueDayOfMonth.HasValue)
                {
                    int day = Math.Min(config.DueDayOfMonth.Value,
                        DateTime.DaysInMonth(year, lastMonthOfQuarter));
                    return new DateOnly(year, lastMonthOfQuarter, day);
                }

                if (config.DueDaysAfterPeriodEnd.HasValue)
                {
                    var lastDay = new DateOnly(year, lastMonthOfQuarter,
                        DateTime.DaysInMonth(year, lastMonthOfQuarter));
                    return lastDay.AddDays(config.DueDaysAfterPeriodEnd.Value);
                }

                return null;
            }

            private async Task AttachChecklistAsync(CompanyTask task, int taskTypeId)
            {
                var template = await _taskRepository.GetActiveChecklistTemplateAsync(taskTypeId);

                if (template == null || !template.Items.Any())
                    return;

                foreach (var item in template.Items.OrderBy(i => i.OrderIndex))
                {
                    task.ChecklistItems.Add(new CompanyTaskChecklistItem
                    {
                        CompanyTask = task,
                        TemplateItemId = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        OrderIndex = item.OrderIndex,
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

    // ==========================================
    // HANDLER: יצירת משימות שנתיות
    // ==========================================

    public class GenerateYearlyTasksCommandHandler
        : IRequestHandler<GenerateYearlyTasksCommand, GenerateTasksResult>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GenerateYearlyTasksCommandHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<GenerateTasksResult> Handle(
            GenerateYearlyTasksCommand request,
            CancellationToken cancellationToken)
        {
            var yearlyConfigs = await _taskRepository
                .GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.Yearly);

            if (!yearlyConfigs.Any())
            {
                return new GenerateTasksResult
                {
                    Success = true,
                    TasksCreated = 0,
                    Message = "לא נמצאו תצורות שנתיות פעילות"
                };
            }

            var activeCompanies = await _taskRepository.GetActiveCompaniesAsync();
            var periodDate = new DateOnly(request.Year, 1, 1);
            var tasksToCreate = new List<CompanyTask>();

            foreach (var company in activeCompanies)
            {
                foreach (var config in yearlyConfigs)
                {
                    var settings = await _taskRepository.GetCompanySettingsAsync(
                        company.Id,
                        config.TaskTypeId);

                    if (settings == null && !config.IsMandatory)
                        continue;

                    if (settings != null && !settings.IsActive)
                        continue;

                    if (await _taskRepository.TaskExistsAsync(
                        company.Id,
                        config.TaskTypeId,
                        periodDate))
                        continue;

                    var task = new CompanyTask
                    {
                        Companyid = company.Id,
                        Tasktypeid = config.TaskTypeId,
                        Period = periodDate,
                        Duedate = CalculateYearlyDueDate(request.Year, config, settings),
                        Status = TaskStatus1.Pending,
                        Priority = settings?.CustomPriority ?? TaskPriority.Normal,
                        Assignedworkerid = settings?.DefaultAssignedWorkerId,
                        Notes = settings?.DefaultNotes,
                        Createdat = DateTime.UtcNow,
                        Updatedat = DateTime.UtcNow
                    };

                    await AttachChecklistAsync(task, config.TaskTypeId);
                    tasksToCreate.Add(task);
                }
            }

            int created = 0;
            if (tasksToCreate.Any())
            {
                created = await _taskRepository.CreateTasksAsync(tasksToCreate);
            }

            return new GenerateTasksResult
            {
                Success = true,
                TasksCreated = created,
                Message = $"נוצרו {created} משימות שנתיות ל-{request.Year}"
            };
        }

        private DateOnly? CalculateYearlyDueDate(
            int year,
            TaskTypeConfiguration config,
            CompanyTaskTypeSettings? settings)
        {
            if (settings?.CustomDueDayOfMonth != null)
            {
                int day = Math.Min(settings.CustomDueDayOfMonth.Value, 31);
                return new DateOnly(year, 12, day);
            }

            if (config.DueDayOfMonth.HasValue)
            {
                int day = Math.Min(config.DueDayOfMonth.Value, 31);
                return new DateOnly(year, 12, day);
            }

            if (config.DueDaysAfterPeriodEnd.HasValue)
            {
                var lastDay = new DateOnly(year, 12, 31);
                return lastDay.AddDays(config.DueDaysAfterPeriodEnd.Value);
            }

            return null;
        }

        //    private async Task AttachChecklistAsync(CompanyTask task, int taskTypeId)
        //    {
        //        var template = await _taskRepository.GetActiveChecklistTemplateAsync(taskTypeId);

        //        if (template == null || !template.Items.Any())
        //            return;

        //        foreach (var item in template.Items.OrderBy(i => i.OrderIndex))
        //        {
        //            task.ChecklistItems.Add(new CompanyTaskChecklistItem
        //            {
        //                CompanyTask = task,
        //                TemplateItemId = item.Id,
        //                Title = item.Title,
        //                Description = item.Description,
        //                OrderIndex = item.OrderIndex,
        //                IsCompleted = false,
        //                CreatedAt = DateTime.UtcNow
        //            });
        //        }
        //    }
        //}
        private async Task AttachChecklistAsync(CompanyTask task, int taskTypeId)
        {
            var template = await _taskRepository.GetActiveChecklistTemplateAsync(taskTypeId);
            if (template == null || !template.Items.Any()) return;

            task.ChecklistItems ??= new List<CompanyTaskChecklistItem>();

            foreach (var item in template.Items.OrderBy(i => i.OrderIndex))
            {
                task.ChecklistItems.Add(new CompanyTaskChecklistItem
                {
                    CompanyTaskId = task.Id, // קישור ה-ID שנוצר בשלב א'
                    Title = item.Title,
                    Description = item.Description,
                    OrderIndex = item.OrderIndex,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
    }
        // ==========================================
        // HANDLER: עדכון סטטוס
        // ==========================================

        public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public UpdateTaskStatusCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<bool> Handle(
                UpdateTaskStatusCommand request,
                CancellationToken cancellationToken)
            {
                var task = await _taskRepository.GetByIdAsync(request.TaskId);

                if (task == null)
                    return false;

                task.Status = request.Status;
                task.Updatedat = DateTime.UtcNow;

                if (request.Status == TaskStatus1.Done || request.Status == TaskStatus1.Paid)
                {
                    task.Completeddate = request.CompletedDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
                }

                await _taskRepository.UpdateAsync(task);
                return true;
            }
        }

        // ==========================================
        // HANDLER: עדכון משימה
        // ==========================================

        public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public UpdateTaskCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<bool> Handle(
                UpdateTaskCommand request,
                CancellationToken cancellationToken)
            {
                var task = await _taskRepository.GetByIdAsync(request.TaskId);

                if (task == null)
                    return false;

                if (request.Status.HasValue)
                    task.Status = request.Status.Value;

                if (request.Priority.HasValue)
                    task.Priority = request.Priority.Value;

                if (request.DueDate.HasValue)
                    task.Duedate = request.DueDate.Value;

                if (request.CompletedDate.HasValue)
                    task.Completeddate = request.CompletedDate.Value;

                if (request.AssignedWorkerId.HasValue)
                    task.Assignedworkerid = request.AssignedWorkerId.Value;

                if (request.Notes != null)
                    task.Notes = request.Notes;

                task.Updatedat = DateTime.UtcNow;

                await _taskRepository.UpdateAsync(task);
                return true;
            }
        }

        // ==========================================
        // HANDLER: הקצאת עובד
        // ==========================================

        public class AssignWorkerToTaskCommandHandler : IRequestHandler<AssignWorkerToTaskCommand, bool>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public AssignWorkerToTaskCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<bool> Handle(
                AssignWorkerToTaskCommand request,
                CancellationToken cancellationToken)
            {
                var task = await _taskRepository.GetByIdAsync(request.TaskId);

                if (task == null)
                    return false;

                task.Assignedworkerid = request.WorkerId;
                task.Updatedat = DateTime.UtcNow;

                await _taskRepository.UpdateAsync(task);
                return true;
            }
        }

        // ==========================================
        // HANDLER: מחיקת משימה
        // ==========================================

        public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public DeleteTaskCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<bool> Handle(
                DeleteTaskCommand request,
                CancellationToken cancellationToken)
            {
                await _taskRepository.DeleteAsync(request.TaskId);
                return true;
            }
        }

        // ==========================================
        // HANDLER: השלמת פריט Checklist
        // ==========================================

        public class CompleteChecklistItemCommandHandler
            : IRequestHandler<CompleteChecklistItemCommand, CompleteChecklistItemResult>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public CompleteChecklistItemCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<CompleteChecklistItemResult> Handle(
                CompleteChecklistItemCommand request,
                CancellationToken cancellationToken)
            {
                var item = await _taskRepository.GetChecklistItemByIdAsync(request.ItemId);

                if (item == null)
                {
                    return new CompleteChecklistItemResult
                    {
                        Success = false,
                        Message = "הפריט לא נמצא"
                    };
                }

                item.IsCompleted = true;
                item.CompletedAt = DateTime.UtcNow;
                item.CompletedByWorkerId = request.CompletedByWorkerId;

                if (!string.IsNullOrEmpty(request.Notes))
                    item.Notes = request.Notes;

                await _taskRepository.UpdateChecklistItemAsync(item);
                await UpdateTaskStatusIfNeededAsync(item.CompanyTaskId);

                return new CompleteChecklistItemResult
                {
                    Success = true,
                    Message = "הפריט סומן כבוצע בהצלחה"
                };
            }

            private async Task UpdateTaskStatusIfNeededAsync(int taskId)
            {
                var task = await _taskRepository.GetByIdAsync(taskId);

                if (task == null || !task.ChecklistItems.Any())
                    return;

                var allCompleted = task.ChecklistItems.All(i => i.IsCompleted);

                if (allCompleted &&
                    (task.Status == TaskStatus1.Pending || task.Status == TaskStatus1.InProgress))
                {
                    task.Status = TaskStatus1.Done;
                    task.Completeddate = DateOnly.FromDateTime(DateTime.UtcNow);
                    task.Updatedat = DateTime.UtcNow;
                    await _taskRepository.UpdateAsync(task);
                }
                else if (task.ChecklistItems.Any(i => i.IsCompleted) &&
                         task.Status == TaskStatus1.Pending)
                {
                    task.Status = TaskStatus1.InProgress;
                    task.Updatedat = DateTime.UtcNow;
                    await _taskRepository.UpdateAsync(task);
                }
            }
        }

        // ==========================================
        // HANDLER: ביטול השלמת פריט Checklist
        // ==========================================

        public class UncompleteChecklistItemCommandHandler
            : IRequestHandler<UncompleteChecklistItemCommand, bool>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public UncompleteChecklistItemCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<bool> Handle(
                UncompleteChecklistItemCommand request,
                CancellationToken cancellationToken)
            {
                var item = await _taskRepository.GetChecklistItemByIdAsync(request.ItemId);

                if (item == null)
                    return false;

                item.IsCompleted = false;
                item.CompletedAt = null;
                item.CompletedByWorkerId = null;

                await _taskRepository.UpdateChecklistItemAsync(item);
                return true;
            }
        }

        // ==========================================
        // HANDLER: הוספת פריט Checklist
        // ==========================================

        public class AddChecklistItemCommandHandler
            : IRequestHandler<AddChecklistItemCommand, AddChecklistItemResult>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public AddChecklistItemCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<AddChecklistItemResult> Handle(
                AddChecklistItemCommand request,
                CancellationToken cancellationToken)
            {
                var task = await _taskRepository.GetByIdAsync(request.TaskId);

                if (task == null)
                {
                    return new AddChecklistItemResult
                    {
                        Success = false,
                        Message = "המשימה לא נמצאה"
                    };
                }

                var orderIndex = request.OrderIndex ??
                    (task.ChecklistItems.Any() ? task.ChecklistItems.Max(i => i.OrderIndex) + 1 : 0);

                var item = new CompanyTaskChecklistItem
                {
                    CompanyTaskId = request.TaskId,
                    Title = request.Title,
                    Description = request.Description,
                    OrderIndex = orderIndex,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                var addedItem = await _taskRepository.AddChecklistItemAsync(item);

                return new AddChecklistItemResult
                {
                    Success = true,
                    ItemId = addedItem.Id,
                    Message = "הפריט נוסף בהצלחה"
                };
            }
        }

        // ==========================================
        // HANDLER: מחיקת פריט Checklist
        // ==========================================

        public class DeleteChecklistItemCommandHandler : IRequestHandler<DeleteChecklistItemCommand, bool>
        {
            private readonly ICompanyTaskRepository _taskRepository;

            public DeleteChecklistItemCommandHandler(ICompanyTaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<bool> Handle(
                DeleteChecklistItemCommand request,
                CancellationToken cancellationToken)
            {
                await _taskRepository.DeleteChecklistItemAsync(request.ItemId);
                return true;
            }
        }
    }


    // ==========================================
    // HANDLER: קבלת משימה לפי ID
    // ==========================================

    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, CompanyTaskDetailDto?>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<CompanyTaskDetailDto?> Handle(
            GetTaskByIdQuery request,
            CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId);

            if (task == null)
                return null;

            return new CompanyTaskDetailDto
            {
                Id = task.Id,
                CompanyId = task.Companyid,
                CompanyName = task.Company?.Name ?? string.Empty,
                CompanyTaxId = task.Company?.Taxid,
                TaskTypeId = task.Tasktypeid,
                TaskTypeName = task.Tasktype?.Name ?? string.Empty,
                TaskTypeCategory = task.Tasktype?.Category.ToString(),
                //TaskTypeDescription = task.Tasktype?.Description.T,
                Period = task.Period,
                Status = task.Status.ToString(),
                Priority = task.Priority,
                DueDate = task.Duedate,
                CompletedDate = task.Completeddate,
                AssignedWorkerId = task.Assignedworkerid,
                AssignedWorkerFirstName = task.Assignedworker?.Firstname,
                AssignedWorkerLastName = task.Assignedworker?.Lastname,
                Notes = task.Notes,
                CreatedAt = task.Createdat,
                UpdatedAt = task.Updatedat,
                ChecklistProgress = CalculateProgress(task.ChecklistItems),
                ChecklistItems = task.ChecklistItems.Select(i => new ChecklistItemDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    OrderIndex = i.OrderIndex,
                    IsCompleted = i.IsCompleted,
                    CompletedAt = i.CompletedAt,
                    CompletedByWorkerId = i.CompletedByWorkerId,
                    CompletedByWorkerName = i.CompletedByWorker?.Firstname,
                    Notes = i.Notes
                }).ToList()
            };
        }

        private ChecklistProgressDto CalculateProgress(
            ICollection<CompanyTaskChecklistItem> items)
        {
            if (items == null || !items.Any())
            {
                return new ChecklistProgressDto
                {
                    Total = 0,
                    Completed = 0
                };
            }

            var completed = items.Count(i => i.IsCompleted);

            return new ChecklistProgressDto
            {
                Total = items.Count,
                Completed = completed
            };
        }
    }

    // ==========================================
    // HANDLER: קבלת משימות של חברה
    // ==========================================

    public class GetCompanyTasksQueryHandler
        : IRequestHandler<GetCompanyTasksQuery, List<CompanyTaskDto>>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GetCompanyTasksQueryHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<CompanyTaskDto>> Handle(
            GetCompanyTasksQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetCompanyTasksAsync(
                request.CompanyId,
                request.Year,
                request.Month,
                request.Status
            );

            return tasks.Select(t => new CompanyTaskDto
            {
                Id = t.Id,
                CompanyId = t.Companyid,
                CompanyName = t.Company?.Name ?? string.Empty,
                TaskTypeId = t.Tasktypeid,
                TaskTypeName = t.Tasktype?.Name ?? string.Empty,
                TaskTypeCategory = t.Tasktype?.Category.ToString(),
                Period = t.Period,
                Status = t.Status.ToString(),
                Priority = t.Priority,
                DueDate = t.Duedate,
                CompletedDate = t.Completeddate,
                AssignedWorkerId = t.Assignedworkerid,
                AssignedWorkerName = t.Assignedworker.Firstname+ t.Assignedworker.Lastname,
                Notes = t.Notes,
                CreatedAt = t.Createdat,
                UpdatedAt = t.Updatedat,
                ChecklistProgress = CalculateProgress(t.ChecklistItems)
            }).ToList();
        }

        private ChecklistProgressDto CalculateProgress(
            ICollection<CompanyTaskChecklistItem> items)
        {
            if (items == null || !items.Any())
            {
                return new ChecklistProgressDto
                {
                    Total = 0,
                    Completed = 0
                };
            }

            var completed = items.Count(i => i.IsCompleted);

            return new ChecklistProgressDto
            {
                Total = items.Count,
                Completed = completed
            };
        }
    }

    // ==========================================
    // HANDLER: קבלת משימות של עובד
    // ==========================================

    public class GetWorkerTasksQueryHandler
        : IRequestHandler<GetWorkerTasksQuery, List<CompanyTaskDto>>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GetWorkerTasksQueryHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<CompanyTaskDto>> Handle(
            GetWorkerTasksQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByWorkerIdAsync(request.WorkerId);

            var filteredTasks = request.Status.HasValue
                ? tasks.Where(t => t.Status == request.Status.Value)
                : tasks;

            return filteredTasks.Select(t => new CompanyTaskDto
            {
                Id = t.Id,
                CompanyId = t.Companyid,
                CompanyName = t.Company?.Name ?? string.Empty,
                TaskTypeId = t.Tasktypeid,
                TaskTypeName = t.Tasktype?.Name ?? string.Empty,
                TaskTypeCategory = t.Tasktype?.Category.ToString(),
                Period = t.Period,
                Status = t.Status.ToString(),
                Priority = t.Priority,
                DueDate = t.Duedate,
                CompletedDate = t.Completeddate,
                AssignedWorkerId = t.Assignedworkerid,
                AssignedWorkerName = t.Assignedworker?.Firstname,
                Notes = t.Notes,
                CreatedAt = t.Createdat,
                UpdatedAt = t.Updatedat,
                ChecklistProgress = CalculateProgress(t.ChecklistItems)
            }).ToList();
        }

        private ChecklistProgressDto CalculateProgress(
            ICollection<CompanyTaskChecklistItem> items)
        {
            if (items == null || !items.Any())
            {
                return new ChecklistProgressDto
                {
                    Total = 0,
                    Completed = 0
                };
            }

            var completed = items.Count(i => i.IsCompleted);

            return new ChecklistProgressDto
            {
                Total = items.Count,
                Completed = completed
            };
        }
    }

    // ==========================================
    // HANDLER: קבלת משימות באיחור
    // ==========================================

    public class GetOverdueTasksQueryHandler
        : IRequestHandler<GetOverdueTasksQuery, List<ActiveCompanyTaskDto>>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GetOverdueTasksQueryHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<ActiveCompanyTaskDto>> Handle(
            GetOverdueTasksQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetOverdueTasksAsync();

            return tasks.Select(t => new ActiveCompanyTaskDto
            {
                Id = t.Id,
                CompanyName = t.Company?.Name ?? string.Empty,
                TaskTypeName = t.Tasktype?.Name ?? string.Empty,
                Category = t.Tasktype?.Category.ToString(),
                Period = t.Period,
                Status = t.Status.ToString(),
                Priority = t.Priority,
                DueDate = t.Duedate,
                AssignedWorkerName = t.Assignedworker?.Firstname,
                IsOverdue = true,
                ChecklistCompletedCount = t.ChecklistItems?.Count(i => i.IsCompleted) ?? 0,
                ChecklistTotalCount = t.ChecklistItems?.Count ?? 0
            }).ToList();
        }
    }

    // ==========================================
    // HANDLER: קבלת משימות לפי סטטוס
    // ==========================================

    public class GetTasksByStatusQueryHandler
        : IRequestHandler<GetTasksByStatusQuery, List<ActiveCompanyTaskDto>>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GetTasksByStatusQueryHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<ActiveCompanyTaskDto>> Handle(
            GetTasksByStatusQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByStatusAsync(request.Status.ToString());

            return tasks.Select(t => new ActiveCompanyTaskDto
            {
                Id = t.Id,
                CompanyName = t.Company?.Name ?? string.Empty,
                TaskTypeName = t.Tasktype?.Name ?? string.Empty,
                Category = t.Tasktype?.Category.ToString(),
                Period = t.Period,
                Status = t.Status.ToString(),
                Priority = t.Priority,
                DueDate = t.Duedate,
                AssignedWorkerName = t.Assignedworker?.Firstname,
                IsOverdue = t.Duedate.HasValue && t.Duedate.Value < DateOnly.FromDateTime(DateTime.Now),
                ChecklistCompletedCount = t.ChecklistItems?.Count(i => i.IsCompleted) ?? 0,
                ChecklistTotalCount = t.ChecklistItems?.Count ?? 0
            }).ToList();
        }
    }

    // ==========================================
    // HANDLER: קבלת פריטי Checklist
    // ==========================================

    public class GetTaskChecklistItemsQueryHandler
        : IRequestHandler<GetTaskChecklistItemsQuery, List<ChecklistItemDto>>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public GetTaskChecklistItemsQueryHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<ChecklistItemDto>> Handle(
            GetTaskChecklistItemsQuery request,
            CancellationToken cancellationToken)
        {
            var items = await _taskRepository.GetTaskChecklistItemsAsync(request.TaskId);

            return items.Select(i => new ChecklistItemDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                OrderIndex = i.OrderIndex,
                IsCompleted = i.IsCompleted,
                CompletedAt = i.CompletedAt,
                CompletedByWorkerId = i.CompletedByWorkerId,
                CompletedByWorkerName = i.CompletedByWorker?.Firstname,
                Notes = i.Notes
            }).ToList();
        }
    }
