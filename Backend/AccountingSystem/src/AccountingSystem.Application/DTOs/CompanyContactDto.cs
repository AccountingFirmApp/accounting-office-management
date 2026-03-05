namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for CompanyContact - אנשי קשר של חברה
/// </summary>
public class CompanyContactDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty; 
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public string? CompanyName { get; set; }
}

/// <summary>
/// DTO ליצירת איש קשר חדש
/// </summary>
public class CreateCompanyContactDto
{
    public int CompanyId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

/// <summary>
/// DTO לעדכון איש קשר
/// </summary>
public class UpdateCompanyContactDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
