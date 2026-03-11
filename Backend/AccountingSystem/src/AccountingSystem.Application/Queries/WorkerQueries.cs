using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
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
// GET WORKER COMPANIES
// ========================================
public class GetWorkerCompaniesQuery : IRequest<List<CompanyDto>>
{
    public int WorkerId { get; set; }

    public GetWorkerCompaniesQuery(int workerId)
    {
        WorkerId = workerId;
    }
}
public class GetWorkerTasksQuery : IRequest<IEnumerable<CompanyTask>>
{
    public int WorkerId { get; set; }

   
}
public class GetWorkersByCompanyQuery : IRequest<IEnumerable<WorkerLookupDto>>
{
    public int CompanyId { get; set; }

    public GetWorkersByCompanyQuery(int companyId)
    {
        CompanyId = companyId;
    }
}