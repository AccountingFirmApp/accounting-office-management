using AccountingSystem.Application.DTOs;
using MediatR;

namespace AccountingSystem.Application.Commands.Workers;

// ========================================
// CREATE WORKER COMMAND
// ========================================
public class CreateWorkerCommand : IRequest<WorkerDto>
{
    public int Firmid { get; set; }
    public int Roleid { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Employeeid { get; set; }
    public bool Isactive { get; set; } = true;
    public DateOnly? Hiredate { get; set; }
}

// ========================================
// UPDATE WORKER COMMAND
// ========================================
public class UpdateWorkerCommand : IRequest<WorkerDto>
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool Isactive { get; set; }
}

// ========================================
// DELETE WORKER COMMAND
// ========================================
public class DeleteWorkerCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeleteWorkerCommand(int id)
    {
        Id = id;
    }
}

// ========================================
// LOGIN COMMAND
// ========================================
public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
// ========================================
// GOOGLE LOGIN COMMAND
// ========================================
public class GoogleLoginCommand : IRequest<LoginResponseDto>
{
    public string GoogleToken { get; set; } = null!;
}