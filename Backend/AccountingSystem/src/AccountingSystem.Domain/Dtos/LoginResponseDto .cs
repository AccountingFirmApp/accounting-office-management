using System;
using System.Collections.Generic;
using System.Linq;
public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } // בשניות
    public WorkerInfoDto Worker { get; set; } = null!;
}

public class WorkerInfoDto
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public int FirmId { get; set; }
}

