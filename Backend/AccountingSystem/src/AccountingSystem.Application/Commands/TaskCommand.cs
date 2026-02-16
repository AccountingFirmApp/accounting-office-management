//using MediatR;

//namespace AccountingSystem.Application.Commands.Tasks;

//public class UpdateTaskStatusCommand : IRequest<Unit>
//{
//    public int TaskId { get; set; }
//    public string Status { get; set; }
//}

using AccountingSystem.Domain.Enums;
using MediatR;
using System;

namespace AccountingSystem.Application.Commands.Tasks
{
    // ==========================================
    // COMMAND: יצירת משימה בודדת
    // ==========================================

    public class CreateTaskCommand : IRequest<CreateTaskResult>
    {
        public int CompanyId { get; set; }
        public int TaskTypeId { get; set; }
        public DateOnly Period { get; set; }
        public DateOnly? CustomDueDate { get; set; }
        public int? AssignedWorkerId { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateTaskResult
    {
        public bool Success { get; set; }
        public int TaskId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ==========================================
    // COMMAND: יצירת משימות חודשיות (אוטומטי)
    // ==========================================

    public class GenerateMonthlyTasksCommand : IRequest<GenerateTasksResult>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    // ==========================================
    // COMMAND: יצירת משימות רבעוניות (אוטומטי)
    // ==========================================

    public class GenerateQuarterlyTasksCommand : IRequest<GenerateTasksResult>
    {
        public int Year { get; set; }
        public int Quarter { get; set; } // 1-4
    }

    // ==========================================
    // COMMAND: יצירת משימות שנתיות (אוטומטי)
    // ==========================================

    public class GenerateYearlyTasksCommand : IRequest<GenerateTasksResult>
    {
        public int Year { get; set; }
    }

    // תוצאה משותפת ליצירה אוטומטית
    public class GenerateTasksResult
    {
        public bool Success { get; set; }
        public int TasksCreated { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ==========================================
    // COMMAND: עדכון סטטוס משימה
    // ==========================================

    public class UpdateTaskStatusCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public TaskStatus1 Status { get; set; }
        public DateOnly? CompletedDate { get; set; }
    }

    // ==========================================
    // COMMAND: עדכון משימה
    // ==========================================

    public class UpdateTaskCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public TaskStatus1? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateOnly? DueDate { get; set; }
        public DateOnly? CompletedDate { get; set; }
        public int? AssignedWorkerId { get; set; }
        public string? Notes { get; set; }
    }

    // ==========================================
    // COMMAND: הקצאת עובד למשימה
    // ==========================================

    public class AssignWorkerToTaskCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public int? WorkerId { get; set; } // null = unassign
    }

    // ==========================================
    // COMMAND: מחיקת משימה
    // ==========================================

    public class DeleteTaskCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
    }

    // ==========================================
    // COMMAND: השלמת פריט Checklist
    // ==========================================

    public class CompleteChecklistItemCommand : IRequest<CompleteChecklistItemResult>
    {
        public int ItemId { get; set; }
        public int CompletedByWorkerId { get; set; }
        public string? Notes { get; set; }
    }

    public class CompleteChecklistItemResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ==========================================
    // COMMAND: ביטול השלמת פריט Checklist
    // ==========================================

    public class UncompleteChecklistItemCommand : IRequest<bool>
    {
        public int ItemId { get; set; }
    }

    // ==========================================
    // COMMAND: הוספת פריט Checklist
    // ==========================================

    public class AddChecklistItemCommand : IRequest<AddChecklistItemResult>
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? OrderIndex { get; set; }
    }

    public class AddChecklistItemResult
    {
        public bool Success { get; set; }
        public int ItemId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ==========================================
    // COMMAND: מחיקת פריט Checklist
    // ==========================================

    public class DeleteChecklistItemCommand : IRequest<bool>
    {
        public int ItemId { get; set; }
    }

    public class ToggleChecklistItemCommand : IRequest<bool>
    {
        public int ItemId { get; set; }
        public int? WorkerId { get; set; }

        // קונסטרקטור (אופציונלי, עוזר ביצירה מהירה)
        public ToggleChecklistItemCommand(int itemId, int? workerId)
        {
            ItemId = itemId;
            WorkerId = workerId;
        }
    }
}