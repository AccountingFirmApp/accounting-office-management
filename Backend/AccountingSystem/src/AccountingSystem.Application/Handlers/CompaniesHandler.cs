﻿using AccountingSystem.Application;
﻿using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Application.Queries.Companies;
using AccountingSystem.Application.Queries.Tasks;
using AccountingSystem.Application.Intrefaces;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using AccountingSystem.Application.Commands.Companies;
using Microsoft.Extensions.Logging;

namespace AccountingSystem.Application.Handlers.Companies;

// ========================================
// GET ALL COMPANIES HANDLER
// ========================================
public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllCompaniesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        int firmId = _currentUserService.GetFirmId();

        var companies = await _unitOfWork.Companies.GetAllByFirmIdAsync(firmId, request.IsActive);

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

public class GetCompanyByWorkerIdQueryHandler : IRequestHandler<GetCompanyByWorkerIdQuery, List<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompanyByWorkerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompanyDto>> Handle(GetCompanyByWorkerIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.Companies.GetByIdWorkerAsync(request.Id);

        if (company == null)
        {
            throw new Exception($"חברה עם ID {request.Id} לא נמצאה");
        }

        return _mapper.Map<List<CompanyDto>> (company);
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

        var result = companies.Select(c => new CompanyWithPendingReportsDto
        {
            Id = c.Id,
            Name = c.Name,
            PendingReportsCount = c.Companyreportconfigs.Count(r =>  true)
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
        var companyExists = await _unitOfWork.Companies.ExistsAsync(request.CompanyId);
        if (!companyExists)
        {
            throw new Exception($"חברה עם ID {request.CompanyId} לא נמצאה");
        }

        // 2. קבל את כל המשימות של החברה
        var tasks = await _unitOfWork.CompanyTasks.GetTasksByCompanyIdAsync(request.CompanyId);

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


    // ========================================
    // CREATE COMPANY COMMAND HANDLER
    // ========================================
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCompanyCommandHandler> _logger;

        public CreateCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreateCompanyCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken ct)
        {
            if (!await _unitOfWork.AccountingFirms.ExistsAsync(request.Firmid))
                throw new Exception($"Firm {request.Firmid} not found");

            var existing = await _unitOfWork.Companies.GetByTaxIdAsync(request.Taxid);
            if (existing != null)
            {
                if (existing.Isactive == true)
                    throw new Exception("Company already exists");

                existing.Isactive = true;
                existing.Name = request.Name;
                existing.Address = request.Address;
                existing.Phone = request.Phone;
                existing.Email = request.Email;
                existing.Notes = request.Notes;
                existing.Updatedat = DateTime.UtcNow;
                await _unitOfWork.Companies.UpdateAsync(existing);

                if (request.RestoreExistingData)
                {
                    var workers = await _unitOfWork.CompanyWorkers
                        .FindAsync(cw => cw.Companyid == existing.Id);
                    foreach (var cw in workers)
                    {
                        var worker = await _unitOfWork.Workers.GetByIdAsync(cw.Workerid);
                        if (worker != null && worker.Isactive == true)
                            cw.Isactive = true;
                    }

                    var configs = await _unitOfWork.CompanyReportConfigs
                        .FindAsync(c => c.Companyid == existing.Id);
                    foreach (var c in configs)
                        c.Isactive = true;

                    var tasks = await _unitOfWork.CompanyTasks
                        .FindAsync(t => t.Companyid == existing.Id);
                    foreach (var t in tasks)
                        t.Isactive = true;
                }

                await _unitOfWork.SaveChangesAsync(ct);
                return _mapper.Map<CompanyDto>(existing);
            }

            var company = new Company
            {
                Name = request.Name,
                Taxid = request.Taxid,
                Firmid = request.Firmid,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                Notes = request.Notes,
                Isactive = true,
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow
            };

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveChangesAsync(ct);
            return _mapper.Map<CompanyDto>(company);
        }
    }
  // ========================================
  // UPDATE
  // ========================================
public class UpdateCompanyCommandHandler
    : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCompanyCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        public async Task<CompanyDto> Handle(UpdateCompanyCommand r, CancellationToken ct)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(r.Id)
                ?? throw new Exception("Company not found");

            company.Name = r.Name;
            company.Address = r.Address;
            company.Phone = r.Phone;
            company.Email = r.Email;
            company.Notes = r.Notes;
            company.Updatedat = DateTime.UtcNow;

            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<CompanyDto>(company);
        }


        //// ========================================
        //// SOFT DELETE
        //// ========================================
        public class DeleteCompanyCommandHandler
            : IRequestHandler<DeleteCompanyCommand, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCompanyCommandHandler(IUnitOfWork uow)
            {
                _unitOfWork = uow;
            }

            public async Task<Unit> Handle(DeleteCompanyCommand r, CancellationToken ct)
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(r.Id)
                    ?? throw new Exception("Company not found");

                company.Isactive = false;
                company.Updatedat = DateTime.UtcNow;
                await _unitOfWork.Companies.UpdateAsync(company);

                var companyWorkers = await _unitOfWork.CompanyWorkers
                    .FindAsync(cw => cw.Companyid == r.Id);
                foreach (var cw in companyWorkers)
                    cw.Isactive = false;

                var configs = await _unitOfWork.CompanyReportConfigs
                    .FindAsync(c => c.Companyid == r.Id);
                foreach (var c in configs)
                    c.Isactive = false;

                // הוספנו את זה - זה היה בהערה לפני
                var tasks = await _unitOfWork.CompanyTasks
                    .FindAsync(t => t.Companyid == r.Id);
                foreach (var t in tasks)
                    t.Isactive = false;

                await _unitOfWork.SaveChangesAsync(ct);
                return Unit.Value;
            }
        }
        //// ========================================
        //// HARD DELETE
        //// ========================================
        public class DeleteCompanyPermanentlyCommandHandler
            : IRequestHandler<DeleteCompanyPermanentlyCommand, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCompanyPermanentlyCommandHandler(IUnitOfWork uow)
            {
                _unitOfWork = uow;
            }

   


            public async Task<Unit> Handle(DeleteCompanyPermanentlyCommand r, CancellationToken ct)
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(r.Id)
                    ?? throw new Exception("Company not found");

                // קודם ReportInstances (תלויים ב-Configs)
                var configIds = await _unitOfWork.CompanyReportConfigs
                    .GetConfigIdsByCompanyIdAsync(r.Id);
                if (configIds.Any())
                    await _unitOfWork.ReportInstances.DeleteByConfigIdsAsync(configIds);

                // מחיקת הטבלאות הקשורות
                await _unitOfWork.CompanyReportConfigs.DeleteByCompanyIdAsync(r.Id);
                await _unitOfWork.CompanyWorkers.DeleteByCompanyIdAsync(r.Id);
                await _unitOfWork.CompanyTasks.DeleteByCompanyIdAsync(r.Id);

                // מחיקת החברה עצמה
                await _unitOfWork.Companies.DeleteAsync(r.Id);

                await _unitOfWork.SaveChangesAsync(ct);
                return Unit.Value;
            }
        }
    }
}