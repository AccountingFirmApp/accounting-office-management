// Application/Handlers/Companies/CompaniesHandler.cs
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application;
using AccountingSystem.Application.Queries.Companies;
using AccountingSystem.Application.Queries.Tasks;

using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
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

    public async AccountingSystem.Domain.Entities.Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
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

    public async AccountingSystem.Domain.Entities.Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
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

    public async AccountingSystem.Domain.Entities.Task<List<CompanyDto>> Handle(GetCompaniesByFirmIdQuery request, CancellationToken cancellationToken)
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

    public AccountingSystem.Domain.Entities.Task<List<CompanyWithPendingReportsDto>> Handle(GetCompaniesByFirmIdQueryWithReport request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }



    // ========================================
    // CREATE COMPANY COMMAND HANDLER
    // ========================================
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async AccountingSystem.Domain.Entities.Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company
            {
                Name = request.Name,
                Taxid = request.TaxId,
                Firmid = request.FirmId,
                //Companycontacts = request.ContactPerson,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                Createdat = DateTime.UtcNow
            };

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CompanyDto>(company);
        }
    }

    // ========================================
    // UPDATE COMPANY COMMAND HANDLER
    // ========================================
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async AccountingSystem.Domain.Entities.Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);

            if (company == null)
            {
                throw new Exception($"חברה עם ID {request.Id} לא נמצאה");
            }

            company.Name = request.Name;
            company.Taxid = request.TaxId;
            //company.Companycontacts = request.ContactPerson;
            company.Phone = request.Phone;
            company.Email = request.Email;
            company.Address = request.Address;
            company.Updatedat = DateTime.UtcNow;

            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CompanyDto>(company);
        }
    }

    // ========================================
    // DELETE COMPANY COMMAND HANDLER
    // ========================================
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async AccountingSystem.Domain.Entities.Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);

            if (company == null)
            {
                throw new Exception($"חברה עם ID {request.Id} לא נמצאה");
            }

            await _unitOfWork.Companies.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
public class GetTasksByCompanyIdQueryHandler : IRequestHandler<GetTasksByCompanyIdQuery, List<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTasksByCompanyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async AccountingSystem.Domain.Entities.Task<List<TaskDto>> Handle(GetTasksByCompanyIdQuery request, CancellationToken cancellationToken)
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
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            CompanyId = t.Companyid,
            TaskTypeId = t.Tasktypeid,
            Period = t.Period.ToDateTime(TimeOnly.MinValue),
            DueDate = t.Duedate.HasValue
? t.Duedate.Value.ToDateTime(TimeOnly.MinValue)
: null,
            CompletedDate = t.Completeddate.HasValue
? t.Completeddate.Value.ToDateTime(TimeOnly.MinValue)
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

}
