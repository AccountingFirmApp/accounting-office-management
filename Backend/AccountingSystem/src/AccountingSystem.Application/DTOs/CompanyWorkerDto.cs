namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for CompanyWorker - שיוך עובד לחברה (Many-to-Many פשוט)
/// </summary>
public class CompanyWorkerDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WorkerId { get; set; }
    public bool Isactive { get; set; }
    public DateTime AssignedAt { get; set; }
    
    public string? CompanyName { get; set; }
    public string? WorkerFirstName { get; set; }
    public string? WorkerLastName { get; set; }
    public string? WorkerFullName => WorkerFirstName != null && WorkerLastName != null 
        ? $"{WorkerFirstName} {WorkerLastName}" 
        : null;
}

/// <summary>
/// DTO לשיוך עובד לחברה
/// </summary>
public class AssignWorkerToCompanyDto
{
    public int CompanyId { get; set; }
    public int WorkerId { get; set; }
}

/// <summary>
/// DTO לעדכון שיוך
/// </summary>
public class UpdateCompanyWorkerDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
