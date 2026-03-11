namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for Company - חברות/לקוחות
/// </summary>
public class CompanyDto
{
    public int Id { get; set; }
    public int FirmId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool Isactive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? FirmName { get; set; }
}

/// <summary>
/// DTO ליצירת חברה חדשה
/// </summary>
public class CreateCompanyDto
{    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון חברה
/// </summary>
public class UpdateCompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool Isactive { get; set; }
}

/// <summary>
/// DTO פשוט לרשימות (לביצועים)
/// </summary>
public class CompanyListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool Isactive { get; set; }
}
