namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for Role - תפקידים כלליים במערכת (Admin, Manager, Employee)
/// </summary>
public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO ליצירת תפקיד חדש
/// </summary>
public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון תפקיד
/// </summary>
public class UpdateRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
