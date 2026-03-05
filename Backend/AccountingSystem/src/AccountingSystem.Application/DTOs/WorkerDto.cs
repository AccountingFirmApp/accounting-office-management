namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for Worker - עובדים/ות (בלי Authentication)
/// </summary>
public class WorkerDto
{
    public int Id { get; set; }
    public int FirmId { get; set; }
    public int RoleId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties (optional - for display)
    public string? FirmName { get; set; }
    public string? RoleName { get; set; }
}

/// <summary>
/// DTO ליצירת עובד חדש
/// </summary>
public class CreateWorkerDto
{
    public int FirmId { get; set; }
    public int RoleId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public DateTime? HireDate { get; set; }
}

/// <summary>
/// DTO לעדכון עובד
/// </summary>
public class UpdateWorkerDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO פשוט לרשימות (לביצועים)
/// </summary>
public class WorkerListDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
public class GoogleLoginRequestDto
{
    public string GoogleToken { get; set; } = null!;
}
public class LoginResponseDto
{
    public string Token { get; set; } 
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } 
    public WorkerInfoDto Worker { get; set; }
}

public class WorkerInfoDto
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } 
            public string Firstname { get; set; } 
    public string Lastname { get; set; }
    public string Email { get; set; } 
    public string RoleName { get; set; } 
    public int FirmId { get; set; }
}
public class WorkerLookupDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}