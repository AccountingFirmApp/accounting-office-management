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

// ========================================
// CREATE COMPANY COMMAND
// ========================================
public class CreateCompanyCommand : IRequest<CompanyDto>
{
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public int FirmId { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

// ========================================
// UPDATE COMPANY COMMAND
// ========================================
public class UpdateCompanyCommand : IRequest<CompanyDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

// ========================================
// DELETE COMPANY COMMAND
// ========================================
public class DeleteCompanyCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeleteCompanyCommand(int id)
    {
        Id = id;
    }
}