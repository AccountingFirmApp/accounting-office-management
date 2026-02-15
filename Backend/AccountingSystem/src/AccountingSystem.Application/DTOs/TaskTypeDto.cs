namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for TaskType - סוגי משימות (קליטת בנקים, בדיקת יתרות)
/// </summary>
public class TaskTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; 
    public int DefaultOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO ליצירת סוג משימה חדש
/// </summary>
public class CreateTaskTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int DefaultOrder { get; set; } = 99;
}

/// <summary>
/// DTO לעדכון סוג משימה
/// </summary>
public class UpdateTaskTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int DefaultOrder { get; set; }
}
