// Application/Handlers/Companies/CompaniesHandler.cs
using AccountingSystem.Application;
using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Application.Queries.Companies;
using AccountingSystem.Application.Queries.Tasks;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AccountingSystem.Application.Handlers.Companies;

// ========================================
// GET ALL COMPANIES HANDLER
// ========================================
public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _unitOfWork.Companies.GetAllAsync();
        return _mapper.Map<List<CompanyDto>>(companies);
    }
}

// ========================================
// GET COMPANY BY ID HANDLER
// ========================================
public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);

        if (company == null)
        {
            throw new Exception($"חברה עם ID {request.Id} לא נמצאה");
        }

        return _mapper.Map<CompanyDto>(company);
    }
}

// ========================================
// GET COMPANIES BY FIRM ID HANDLER
// ========================================
public class GetCompaniesByFirmIdQueryHandler : IRequestHandler<GetCompaniesByFirmIdQuery, List<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompaniesByFirmIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompanyDto>> Handle(GetCompaniesByFirmIdQuery request, CancellationToken cancellationToken)
    {
        var companies = await _unitOfWork.Companies.GetCompaniesByFirmIdAsync(request.FirmId);
        return _mapper.Map<List<CompanyDto>>(companies);
    }
}

// ========================================
// GET COMPANIES BY FIRM ID WITH REPORTS HANDLER
// ========================================
public class GetCompaniesByFirmIdQueryWithReportHandler
    : IRequestHandler<GetCompaniesByFirmIdQueryWithReport, List<CompanyWithPendingReportsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompaniesByFirmIdQueryWithReportHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompanyWithPendingReportsDto>> Handle(GetCompaniesByFirmIdQueryWithReport request, CancellationToken cancellationToken)
    {
        var companies = await _unitOfWork.Companies.GetCompaniesByFirmIdAsync(request.FirmId);

        // המרה ל־DTO מתאים
        var result = companies.Select(c => new CompanyWithPendingReportsDto
        {
            Id = c.Id,
            Name = c.Name,
            PendingReportsCount = c.Companyreportconfigs.Count(r => /* תנאי לדוחות ממתינים */ true)
        }).ToList();

        return result;
    }

}


public class GetTasksByCompanyIdQueryHandler : IRequestHandler<GetTasksByCompanyIdQuery, List<CompanyTaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTasksByCompanyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompanyTaskDto>> Handle(GetTasksByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        // 1. בדוק שהחברה קיימת
        var companyExists = await _unitOfWork.Companies.ExistsAsync(request.CompanyId);
        if (!companyExists)
        {
            throw new Exception($"חברה עם ID {request.CompanyId} לא נמצאה");
        }

        // 2. קבל את כל המשימות של החברה
        var tasks = await _unitOfWork.Tasks.GetTasksByCompanyIdAsync(request.CompanyId);

        // 3. המר ל-DTOs
        var taskDtos = tasks.Select(t => new CompanyTaskDto
        {
            Id = t.Id,
            CompanyId = t.Companyid,
            TaskTypeId = t.Tasktypeid,
            Period = t.Period,
            DueDate = t.Duedate.HasValue
? t.Duedate
: null,
            CompletedDate = t.Completeddate.HasValue
? t.Completeddate
: null,
            AssignedWorkerId = t.Assignedworkerid,
            Notes = t.Notes,
            Status = t.Status.ToString(),
            CreatedAt = t.Createdat ?? DateTime.MinValue,
            UpdatedAt = t.Updatedat ?? DateTime.MinValue,

            CompanyName = t.Company?.Name ?? string.Empty,
            TaskTypeName = t.Tasktype?.Name ?? string.Empty,
            AssignedWorkerName = t.Assignedworker != null
                ? $"{t.Assignedworker.Firstname} {t.Assignedworker.Lastname}"
                : null
        }).ToList();

        return taskDtos;
    }
    public class CompleteChecklistItemCommandHandler : IRequestHandler<CompleteChecklistItemCommand, CompleteChecklistItemResult>
    {
        private readonly IChecklistItemRepository _repository; // בהנחה שיש לך Repository כזה
        private readonly IUnitOfWork _unitOfWork;

        public CompleteChecklistItemCommandHandler(IChecklistItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CompleteChecklistItemResult> Handle(CompleteChecklistItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.ItemId);

            if (item == null)
                return new CompleteChecklistItemResult { Success = false, Message = "הפריט לא נמצא" };

            // עדכון השדות
            item.IsCompleted = true;
            item.CompletedAt = DateTime.UtcNow;
            item.CompletedByWorkerId = request.CompletedByWorkerId;
            item.Notes = request.Notes;

            _repository.Update(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CompleteChecklistItemResult { Success = true, Message = "הפריט הושלם בהצלחה" };
        }
    }

}
