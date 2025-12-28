namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for Task - משימות
/// </summary>
public class TaskDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int TaskTypeId { get; set; }
    public DateTime Period { get; set; }
    public string Status { get; set; } = string.Empty; // Pending, InProgress, Done, Paid, NotRequired
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int? AssignedWorkerId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation (optional)
    public string? CompanyName { get; set; }
    public string? TaskTypeName { get; set; }
    public string? AssignedWorkerName { get; set; }
}

/// <summary>
/// DTO ליצירת משימה חדשה
/// </summary>
public class CreateTaskDto
{
    public int CompanyId { get; set; }
    public int TaskTypeId { get; set; }
    public DateTime Period { get; set; }
    public DateTime? DueDate { get; set; }
    public int? AssignedWorkerId { get; set; }
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון משימה
/// </summary>
public class UpdateTaskDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int? AssignedWorkerId { get; set; }
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// DTO מורחב - עם כל הפרטים
/// </summary>
public class TaskDetailDto
{
    public int Id { get; set; }
    
    // Company info
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyTaxId { get; set; } = string.Empty;
    
    // Task Type info
    public int TaskTypeId { get; set; }
    public string TaskTypeName { get; set; } = string.Empty;
    public string TaskTypeCategory { get; set; } = string.Empty;
    
    // Task data
    public DateTime Period { get; set; }
    public string PeriodFormatted => Period.ToString("MM/yyyy"); // 09/2025
    public string Status { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    // Worker info
    public int? AssignedWorkerId { get; set; }
    public string? AssignedWorkerFirstName { get; set; }
    public string? AssignedWorkerLastName { get; set; }
    public string? AssignedWorkerFullName => AssignedWorkerFirstName != null && AssignedWorkerLastName != null 
        ? $"{AssignedWorkerFirstName} {AssignedWorkerLastName}" 
        : null;
    
    public string Notes { get; set; } = string.Empty;
    
    // Calculated fields
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now.Date && Status != "Done" && Status != "Paid";
}

/// <summary>
/// DTO לעדכון סטטוס מהיר
/// </summary>
public class UpdateTaskStatusDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? CompletedDate { get; set; }
}

/// <summary>
/// DTO להקצאת משימה לעובד
/// </summary>
public class AssignTaskToWorkerDto
{
    public int TaskId { get; set; }
    public int WorkerId { get; set; }
}

/// <summary>
/// DTO לרשימת משימות פעילות
/// </summary>
public class ActiveTaskDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string TaskTypeName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Period { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public string? AssignedWorkerName { get; set; }
    public bool IsOverdue { get; set; }
}
