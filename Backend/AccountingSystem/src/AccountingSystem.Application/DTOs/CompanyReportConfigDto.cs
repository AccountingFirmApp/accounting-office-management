namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for CompanyReportConfig - הגדרת דיווחים פר-לקוח
/// </summary>
public class CompanyReportConfigDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int ReportTypeId { get; set; }
    public int FrequencyId { get; set; }
    public short? DayOfMonth { get; set; } 
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Year { get; set; }

    // Navigation (optional)
    public string? CompanyName { get; set; }
    public string? ReportTypeName { get; set; }
    public string? ReportTypeShortCode { get; set; }
    public string? FrequencyName { get; set; }
}

/// <summary>
/// DTO ליצירת הגדרת דיווח חדשה
/// </summary>
public class CreateCompanyReportConfigDto
{
    public int CompanyId { get; set; }
    public int ReportTypeId { get; set; }
    public int FrequencyId { get; set; }
    public short? DayOfMonth { get; set; }
    public int Year { get; set; }
}

/// <summary>
/// DTO לעדכון הגדרת דיווח
/// </summary>
public class UpdateCompanyReportConfigDto
{
    public int FrequencyId { get; set; }
    public short? DayOfMonth { get; set; }
    public bool? IsActive { get; set; }

    /// <summary>
    /// DTO מורחב - עם כל הפרטים
    /// </summary>
    public class CompanyReportConfigDetailDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int ReportTypeId { get; set; }
        public string ReportTypeName { get; set; } = string.Empty;
        public string ReportTypeShortCode { get; set; } = string.Empty;
        public int FrequencyId { get; set; }
        public string FrequencyName { get; set; } = string.Empty;
        public short? DayOfMonth { get; set; }
        public bool IsActive { get; set; }
    }
}
