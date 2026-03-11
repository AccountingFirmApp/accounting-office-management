
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace AccountingSystem.Application.DTOs.Tasks
{
    // ==========================================
    // DTOs בסיסיים למשימות
    // ==========================================

    /// <summary>
    /// DTO בסיסי למשימה - לרשימות ותצוגה כללית
    /// </summary>
    public class CompanyTaskDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; } = string.Empty;
        public string? TaskTypeCategory { get; set; }

        public DateOnly Period { get; set; }
        public string PeriodFormatted => Period.ToString("MM/yyyy"); // 09/2025

        public string Status { get; set; } = string.Empty;
        public string StatusDisplay => Status.ToString();

        public TaskPriority Priority { get; set; }
        public string PriorityDisplay => Priority.ToString();

        public DateOnly? DueDate { get; set; }
        public DateOnly? CompletedDate { get; set; }

        public int? AssignedWorkerId { get; set; }
        public string? AssignedWorkerName { get; set; }

        public string? Notes { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CompanyTaskChecklistItemDto> ChecklistItems { get; set; } = new();
        // Checklist progress
        public ChecklistProgressDto ChecklistProgress { get; set; } = new();

        // Calculated fields
        public bool IsOverdue => DueDate.HasValue
            && DueDate.Value < DateOnly.FromDateTime(DateTime.Now)
            && Status != "Done" && Status != "Paid";
    }

    /// <summary>
    /// DTO מפורט למשימה - לתצוגת פרטים מלאים
    /// </summary>
    /// 
    public class CompanyTaskDetailDto : CompanyTaskDto
    {

        public string? CompanyTaxId { get; set; }
        public string? TaskTypeDescription { get; set; }
        public string? AssignedWorkerFirstName { get; set; }
        public string? AssignedWorkerLastName { get; set; }

        public string? AssignedWorkerFullName =>
            !string.IsNullOrEmpty(AssignedWorkerFirstName) && !string.IsNullOrEmpty(AssignedWorkerLastName)
                ? $"{AssignedWorkerFirstName} {AssignedWorkerLastName}"
                : null;

        public int DaysUntilDue
        {
            get
            {
                if (!DueDate.HasValue) return 0;
                var today = DateOnly.FromDateTime(DateTime.Now);
                return DueDate.Value.DayNumber - today.DayNumber;
            }
        }
    }
    

    /// <summary>
    /// DTO קל משקל לרשימות פעילות ודשבורד
    /// </summary>
    public class ActiveCompanyTaskDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string TaskTypeName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateOnly Period { get; set; }
        public string Status { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public DateOnly? DueDate { get; set; }
        public string? AssignedWorkerName { get; set; }
        public bool IsOverdue { get; set; }
        public int ChecklistCompletedCount { get; set; }
        public int ChecklistTotalCount { get; set; }
    }

    // ==========================================
    // DTOs ליצירה ועדכון
    // ==========================================

    /// <summary>
    /// DTO ליצירת משימה חדשה
    /// </summary>
    public class CreateCompanyTaskDto
    {
        public int CompanyId { get; set; }
        public int TaskTypeId { get; set; }
        public DateOnly Period { get; set; }
        public DateOnly? CustomDueDate { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;
        public int? AssignedWorkerId { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO לעדכון משימה
    /// </summary>
    public class UpdateCompanyTaskDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public TaskPriority? Priority { get; set; }
        public DateOnly? DueDate { get; set; }
        public DateOnly? CompletedDate { get; set; }
        public int? AssignedWorkerId { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO לעדכון סטטוס מהיר
    /// </summary>
    public class UpdateCompanyTaskStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? CompletedDate { get; set; }
    }

    /// <summary>
    /// DTO להקצאת עובד למשימה
    /// </summary>
    public class AssignCompanyTaskToWorkerDto
    {
        public int TaskId { get; set; }
        public int? WorkerId { get; set; } // null = unassign
    }

    // ==========================================
    // DTOs ל-Checklist
    // ==========================================

    /// <summary>
    /// DTO להתקדמות Checklist
    /// </summary>
    public class ChecklistProgressDto
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Percentage => Total > 0 ? (Completed * 100 / Total) : 0;
        public int Remaining => Total - Completed;
        public bool IsComplete => Total > 0 && Completed == Total;
    }

    /// <summary>
    /// DTO לפריט Checklist
    /// </summary>
    public class ChecklistItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? CompletedByWorkerId { get; set; }
        public string? CompletedByWorkerName { get; set; }

        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO להשלמת פריט Checklist
    /// </summary>
    public class CompleteChecklistItemDto
    {
        public int ItemId { get; set; }
        public int CompletedByWorkerId { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO להוספת פריט Checklist
    /// </summary>
    public class AddChecklistItemDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? OrderIndex { get; set; }
    }

    /// <summary>
    /// DTO לעדכון פריט Checklist
    /// </summary>
    public class UpdateChecklistItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? OrderIndex { get; set; }
        public string? Notes { get; set; }
    }

    // ==========================================
    // DTOs לאצווה (Batch Operations)
    // ==========================================

    /// <summary>
    /// DTO ליצירת מספר משימות בבת אחת
    /// </summary>
    public class CreateBulkTasksDto
    {
        public List<int> CompanyIds { get; set; } = new();
        public int TaskTypeId { get; set; }
        public DateOnly Period { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;
    }

    /// <summary>
    /// DTO לעדכון מספר משימות
    /// </summary>
    public class UpdateBulkTasksDto
    {
        public List<int> TaskIds { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public TaskPriority? Priority { get; set; }
        public int? AssignedWorkerId { get; set; }
    }

    public class CompanyTaskChecklistItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int OrderIndex { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? CompletedByWorkerName { get; set; }
    }
    public class ChecklistTemplateDto
    {
        public int Id { get; set; }
        public int TaskTypeId { get; set; }
        public List<ChecklistTemplateItemDto> Items { get; set; } = new();
    }

    public class ChecklistTemplateItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsOptional { get; set; }
    }
    public class CompanyTaskConfigDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }

        // נתונים מהקונפיגורציה (יכולים להיות null אם עדיין לא הוגדרו)
        public int? ConfigurationId { get; set; }
        public int? assignedWorkerId { get; set; }
        public string? WorkerName { get; set; }
        public RecurrenceType Frequency { get; set; } = RecurrenceType.Monthly;
        public int DueDay { get; set; } = 15;   // ברירת מחדל 15 לחודש
        public bool IsActive { get; set; } = false; // כברירת מחדל לא משובץ
    }

}