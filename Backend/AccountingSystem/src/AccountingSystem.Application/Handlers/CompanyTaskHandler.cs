

using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Application.Queries.Tasks;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
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

            task.ChecklistItems ??= new List<CompanyTaskChecklistItem>();

            foreach (var item in template.Items.OrderBy(i => i.OrderIndex))
            {
                var newItem = new CompanyTaskChecklistItem
                {
                    CompanyTaskId = task.Id,
                    Title = item.Title,
                    Description = item.Description,
                    OrderIndex = item.OrderIndex,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                task.ChecklistItems.Add(newItem);
            }

        }
    }
    

    public abstract class TaskGeneratorBase
    {
        protected readonly ICompanyTaskRepository _taskRepository;

        protected TaskGeneratorBase(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        protected async Task<bool> ShouldCreateTaskAsync(int companyId, TaskTypeConfiguration config)
        {
            var settings = await _taskRepository.GetCompanySettingsAsync(companyId, config.TaskTypeId);

            if (settings == null) return config.IsMandatory;

           
            return settings.Isactive;
        }

        protected async Task<CompanyTask> CreateTaskFromConfigAsync(int companyId, TaskTypeConfiguration config, DateOnly period)
        {
            var settings = await _taskRepository.GetCompanySettingsAsync(companyId, config.TaskTypeId);

            var task = new CompanyTask
            {
                Companyid = companyId,
                Tasktypeid = config.TaskTypeId,
                Period = period,
                Duedate = CalculateDueDate(period, config, settings),
                Status = TaskStatus1.Pending,

                Assignedworkerid = settings?.Assignedworkerid,

                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                ChecklistItems = new List<CompanyTaskChecklistItem>()
            };

            await AttachChecklistAsync(task, config.TaskTypeId);
            return task;
        }

        protected DateOnly? CalculateDueDate(DateOnly period, TaskTypeConfiguration config, CompanyTaskConfiguration? settings)
        {
            int daysInMonth = DateTime.DaysInMonth(period.Year, period.Month);
            int dayToUse;

            if (settings != null && settings.Dueday > 0)
            {
                dayToUse = settings.Dueday;
            }
            else
            {
                dayToUse = config.DueDayOfMonth ?? 15; 
            }

            return new DateOnly(period.Year, period.Month, Math.Min(dayToUse, daysInMonth));
        }

        protected async Task AttachChecklistAsync(CompanyTask task, int taskTypeId)
        {
            var template = await _taskRepository.GetActiveChecklistTemplateAsync(taskTypeId);
            if (template == null || template.Items == null) return;

            foreach (var item in template.Items.OrderBy(i => i.OrderIndex))
            {
                task.ChecklistItems.Add(new CompanyTaskChecklistItem
                {
                    Title = item.Title,
                    Description = item.Description,
                    OrderIndex = item.OrderIndex,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
    }
    public class GenerateYearlyTasksCommandHandler
     : TaskGeneratorBase, IRequestHandler<GenerateYearlyTasksCommand, GenerateTasksResult>
    {
        public GenerateYearlyTasksCommandHandler(ICompanyTaskRepository repo) : base(repo) { }

        public async Task<GenerateTasksResult> Handle(GenerateYearlyTasksCommand request, CancellationToken ct)
        {
            var configs = await _taskRepository.GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.Yearly);
            var companies = await _taskRepository.GetActiveCompaniesAsync();

            // תאריך מייצג לשנה כולה
            var periodDate = new DateOnly(request.Year, 1, 1);

            int createdCount = 0;
            foreach (var company in companies)
            {
                foreach (var config in configs)
                {
                    if (!await ShouldCreateTaskAsync(company.Id, config)) continue;
                    if (await _taskRepository.TaskExistsAsync(company.Id, config.TaskTypeId, periodDate)) continue;

                    var task = await CreateTaskFromConfigAsync(company.Id, config, periodDate);
                    await _taskRepository.AddAsync(task);
                    createdCount++;
                }
            }
            return new GenerateTasksResult { Success = true, TasksCreated = createdCount };
        }
    }
    public class GenerateQuarterlyTasksCommandHandler
    : TaskGeneratorBase, IRequestHandler<GenerateQuarterlyTasksCommand, GenerateTasksResult>
    {
        public GenerateQuarterlyTasksCommandHandler(ICompanyTaskRepository repo) : base(repo) { }

        public async Task<GenerateTasksResult> Handle(GenerateQuarterlyTasksCommand request, CancellationToken ct)
        {
            var configs = await _taskRepository.GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.Quarterly);
            var companies = await _taskRepository.GetActiveCompaniesAsync();

            // חישוב חודש תחילת הרבעון (רבעון 1 -> ינואר, רבעון 2 -> אפריל וכו')
            int startMonth = (request.Quarter - 1) * 3 + 1;
            var periodDate = new DateOnly(request.Year, startMonth, 1);

            int createdCount = 0;
            foreach (var company in companies)
            {
                foreach (var config in configs)
                {
                    if (!await ShouldCreateTaskAsync(company.Id, config)) continue;
                    if (await _taskRepository.TaskExistsAsync(company.Id, config.TaskTypeId, periodDate)) continue;

                    var task = await CreateTaskFromConfigAsync(company.Id, config, periodDate);
                    await _taskRepository.AddAsync(task);
                    createdCount++;
                }
            }
            return new GenerateTasksResult { Success = true, TasksCreated = createdCount };
        }
    }

    public class GenerateBiMonthlyTasksCommandHandler
    : TaskGeneratorBase, IRequestHandler<GenerateBiMonthlyTasksCommand, GenerateTasksResult>
    {
        public GenerateBiMonthlyTasksCommandHandler(ICompanyTaskRepository repo) : base(repo) { }

       
        public async Task<GenerateTasksResult> Handle(GenerateBiMonthlyTasksCommand request, CancellationToken ct)
        {
            var configs = await _taskRepository.GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.BiMonthly);
            var companies = await _taskRepository.GetActiveCompaniesAsync();

            

            int periodNumber = (request.Month + 1) / 2; // למשל מרץ (3) -> (4)/2 = תקופה 2
            int startMonth = (periodNumber * 2) - 1;    // תקופה 2 -> (4)-1 = חודש 3 (מרץ)
            var periodDate = new DateOnly(request.Year, startMonth, 1);


            int createdCount = 0;
            foreach (var company in companies)
            {
                foreach (var config in configs)
                {
                    if (!await ShouldCreateTaskAsync(company.Id, config)) continue;

                    if (await _taskRepository.TaskExistsAsync(company.Id, config.TaskTypeId, periodDate))
                    {
                        continue;
                    }

                    var task = await CreateTaskFromConfigAsync(company.Id, config, periodDate);
                    await _taskRepository.AddAsync(task);
                    createdCount++;
                }
            }
            return new GenerateTasksResult { Success = true, TasksCreated = createdCount };
        }
    }
        public class GenerateMonthlyTasksCommandHandler
    : TaskGeneratorBase, IRequestHandler<GenerateMonthlyTasksCommand, GenerateTasksResult>
    {
        public GenerateMonthlyTasksCommandHandler(ICompanyTaskRepository repo) : base(repo) { }

        public async Task<GenerateTasksResult> Handle(GenerateMonthlyTasksCommand request, CancellationToken ct)
        {
            // 1. שליפת כל סוגי המשימות החודשיות מהמטריצה הכללית
            var configs = await _taskRepository.GetActiveConfigurationsByRecurrenceAsync(RecurrenceType.Monthly);
            var companies = await _taskRepository.GetActiveCompaniesAsync();

            // הגדרת התאריך המייצג (למשל: 01/03/2024)
            var periodDate = new DateOnly(request.Year, request.Month, 1);
            int createdCount = 0;

            foreach (var company in companies)
            {
                foreach (var config in configs)
                {
                    // בדיקה בטבלת ה-UI (CompanyTaskConfiguration) האם ליצור
                    if (!await ShouldCreateTaskAsync(company.Id, config)) continue;

                    // מניעת כפילויות
                    if (await _taskRepository.TaskExistsAsync(company.Id, config.TaskTypeId, periodDate)) continue;

                    // יצירה ושמירה (הלוגיקה ב-Base Class לוקחת מהטבלה הנכונה!)
                    var task = await CreateTaskFromConfigAsync(company.Id, config, periodDate);
                    await _taskRepository.AddAsync(task);
                    createdCount++;
                }
            }
            return new GenerateTasksResult { Success = true, TasksCreated = createdCount };
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
          
            ChecklistItems = task.ChecklistItems.OrderBy(i => i.OrderIndex).Select(i => new CompanyTaskChecklistItemDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                OrderIndex = i.OrderIndex,
                IsCompleted = i.IsCompleted,
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
            AssignedWorkerName = t.Assignedworker.Firstname + t.Assignedworker.Lastname,
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
public class GetChecklistTemplateByTaskTypeHandler : IRequestHandler<GetChecklistTemplateByTaskTypeQuery, ChecklistTemplateDto>
{
    private readonly ICompanyTaskRepository _repository;

    public GetChecklistTemplateByTaskTypeHandler(ICompanyTaskRepository repository) => _repository = repository;

    public async Task<ChecklistTemplateDto> Handle(GetChecklistTemplateByTaskTypeQuery request, CancellationToken ct)
    {
        var template = await _repository.GetChecklistTemplateByTaskTypeAsync(request.TaskTypeId);
        if (template == null) return new ChecklistTemplateDto { TaskTypeId = request.TaskTypeId };

        return new ChecklistTemplateDto
        {
            Id = template.Id,
            TaskTypeId = template.TaskTypeId,
            Items = template.Items.Select(i => new ChecklistTemplateItemDto
            {
                Title = i.Title,
                Description = i.Description,
                OrderIndex = i.OrderIndex,
                IsOptional = i.IsOptional
            }).ToList()
        };
    }
    public class SaveChecklistTemplateCommandHandler : IRequestHandler<SaveChecklistTemplateCommand, bool>
    {
        private readonly ICompanyTaskRepository _taskRepository;

        public SaveChecklistTemplateCommandHandler(ICompanyTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(SaveChecklistTemplateCommand request, CancellationToken cancellationToken)
        {
            // 1. ננסה להביא את התבנית הקיימת עבור סוג המשימה הזה
            var template = await _taskRepository.GetChecklistTemplateByTaskTypeAsync(request.TaskTypeId);

            // 2. אם לא קיימת תבנית, ניצור אחת חדשה
            if (template == null)
            {
                template = new TaskChecklistTemplate
                {
                    TaskTypeId = request.TaskTypeId,
                    Name = $"Template for Task Type {request.TaskTypeId}", // אפשר לשפר את השם בהמשך
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
            }
            else
            {
                template.UpdatedAt = DateTime.UtcNow;
            }
            var newItems = request.Items.Select(itemDto => new TaskChecklistTemplateItem
            {
                Title = itemDto.Title,
                Description = itemDto.Description,
                OrderIndex = itemDto.OrderIndex,
                IsOptional = itemDto.IsOptional,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            try
            {
                await _taskRepository.UpdateChecklistTemplateAsync(template, newItems);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}


public class GetTaskTypesHandler : IRequestHandler<GetTaskTypesQuery, List<TaskTypeDto>>
{
    private readonly ICompanyTaskRepository _repository;

    public GetTaskTypesHandler(ICompanyTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TaskTypeDto>> Handle(GetTaskTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _repository.GetTaskTypesAsync();

        return types.Select(t => new TaskTypeDto
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();
    }

}

public class GetTaskMatrixHandler : IRequestHandler<GetTaskMatrixQuery, List<CompanyTaskConfigDto>>
{
    private readonly ICompanyRepository _companyRepo;
    private readonly ITaskTypeRepository _taskTypeRepo; // ודאי שקיים ממשק כזה
    private readonly ITaskConfigurationRepository _configRepo;
    private readonly IHttpContextAccessor _httpContextAccessor; // הוספה

    public GetTaskMatrixHandler(
        ICompanyRepository companyRepo,
        ITaskTypeRepository taskTypeRepo,
        ITaskConfigurationRepository configRepo,
        IHttpContextAccessor httpContextAccessor)
    {
        _companyRepo = companyRepo;
        _taskTypeRepo = taskTypeRepo;
        _configRepo = configRepo;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task<List<CompanyTaskConfigDto>> Handle(GetTaskMatrixQuery request, CancellationToken cancellationToken)
    {
        var firmIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == "FirmId")?.Value;

        if (string.IsNullOrEmpty(firmIdClaim))
            throw new UnauthorizedAccessException("FirmId not found in token.");

        int firmId = int.Parse(firmIdClaim);
        var companies = await _companyRepo.GetAllByFirmIdAsync(firmId);
        var taskTypes = await _taskTypeRepo.GetAllAsync();
        var existingConfigs = await _configRepo.GetAllWithWorkersAsync();

        // 2. בניית המטריצה (Cross Join)
        var matrix = from c in companies
                     from tt in taskTypes
                     join conf in existingConfigs
                        on new { CId = c.Id, TtId = tt.Id }
                        equals new { CId = conf.Companyid, TtId = conf.Tasktypeid } into joined
                     from conf in joined.DefaultIfEmpty()
                     select new CompanyTaskConfigDto
                     {
                         CompanyId = c.Id,
                         CompanyName = c.Name,
                         TaskTypeId = tt.Id,
                         TaskTypeName = tt.Name,
                         ConfigurationId = conf?.Id,
                         assignedWorkerId = conf?.Assignedworkerid,
                         WorkerName = conf?.Assignedworker?.Firstname ?? "לא שובץ",
                         Frequency = conf?.Frequency ?? RecurrenceType.Monthly,
                         DueDay = conf?.Dueday ?? 15,
                         IsActive = conf?.Isactive ?? false
                     };

        return matrix.OrderBy(m => m.CompanyName).ThenBy(m => m.TaskTypeName).ToList();
    }
    public class SaveTaskConfigurationHandler : IRequestHandler<SaveTaskConfigurationCommand, bool>
    {
        private readonly ITaskConfigurationRepository _repository;

        public SaveTaskConfigurationHandler(ITaskConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(SaveTaskConfigurationCommand request, CancellationToken cancellationToken)
        {
            var existingConfig = await _repository.GetByCompanyAndTypeAsync(request.CompanyId, request.TaskTypeId);

            if (existingConfig == null)
            {
                var newConfig = new CompanyTaskConfiguration
                {
                    Companyid = request.CompanyId,
                    Tasktypeid = request.TaskTypeId,
                    Assignedworkerid = request.AssignedWorkerId,
                    Frequency = (RecurrenceType)request.Frequency,
                    Dueday = request.DueDay,
                    Isactive = request.IsActive,
                    Createdat = DateTime.UtcNow
                };
                await _repository.AddAsync(newConfig);
            }
            else
            {
                existingConfig.Assignedworkerid = request.AssignedWorkerId;
                existingConfig.Frequency = (RecurrenceType)request.Frequency;
                existingConfig.Dueday = request.DueDay;
                existingConfig.Isactive = request.IsActive;
                existingConfig.Updatedat = DateTime.UtcNow;

                _repository.UpdateAsync(existingConfig);
            }

            await _repository.SaveChangesAsync();
            return true;
        }
    }
}