namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for ReportType - סוגי דיווחים (מע"מ, ביטוח לאומי, ניכוי מס)
/// </summary>
public class ReportType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OfficialUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO ליצירת סוג דיווח חדש
/// </summary>
public class CreateReportTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OfficialUrl { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון סוג דיווח
/// </summary>
public class UpdateReportTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OfficialUrl { get; set; } = string.Empty;
}
