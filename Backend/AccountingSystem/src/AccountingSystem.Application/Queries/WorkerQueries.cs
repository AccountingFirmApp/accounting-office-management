// Application/Queries/Workers/WorkerQueries.cs
using AccountingSystem.Application.DTOs;
using MediatR;

namespace AccountingSystem.Application.Queries.Workers;

// ========================================
// GET ALL WORKERS
// ========================================
public class GetAllWorkersQuery : IRequest<List<WorkerDto>>
{
}

// ========================================
// GET WORKER BY ID
// ========================================
public class GetWorkerByIdQuery : IRequest<WorkerDto>
{
    public int Id { get; set; }

    public GetWorkerByIdQuery(int id)
    {
        Id = id;
    }
}

// ========================================
// GET WORKERS BY FIRM ID
// ========================================
public class GetWorkersByFirmIdQuery : IRequest<List<WorkerDto>>
{
    public int FirmId { get; set; }

    public GetWorkersByFirmIdQuery(int firmId)
    {
        FirmId = firmId;
    }
}

// ========================================
// CREATE WORKER COMMAND
// ========================================
public class CreateWorkerCommand : IRequest<WorkerDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int FirmId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
}

// ========================================
// UPDATE WORKER COMMAND
// ========================================
public class UpdateWorkerCommand : IRequest<WorkerDto>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
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
