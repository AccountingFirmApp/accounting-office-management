using AccountingSystem.Application.DTOs;
using MediatR;

namespace AccountingSystem.Application.Queries.Companies;

// ========================================
// GET ALL COMPANIES
// ========================================
public class GetAllCompaniesQuery : IRequest<List<CompanyDto>>
{
}

// ========================================
// GET COMPANY BY ID
// ========================================
public class GetCompanyByIdQuery : IRequest<CompanyDto>
{
    public int Id { get; set; }

    public GetCompanyByIdQuery(int id)
    {
        Id = id;
    }
}

// ========================================
// GET COMPANY BY Worker ID
// ========================================
public class GetCompanyByWorkerIdQuery : IRequest<List<CompanyDto>>
{
    public int Id { get; set; }

    public GetCompanyByWorkerIdQuery(int id)
    {
        Id = id;
    }
}
// ========================================
// GET COMPANIES BY FIRM ID
// ========================================
public class GetCompaniesByFirmIdQuery : IRequest<List<CompanyDto>>
{
    public int FirmId { get; set; }

    public GetCompaniesByFirmIdQuery(int firmId)
    {
        FirmId = firmId;
    }
}

// ========================================
// GET COMPANIES BY FIRM ID WITH PENDING REPORTS
// ========================================
public class GetCompaniesByFirmIdQueryWithReport : IRequest<List<CompanyWithPendingReportsDto>>
{
    public int FirmId { get; set; }

    public GetCompaniesByFirmIdQueryWithReport(int firmId)
    {
        FirmId = firmId;
    }
}

