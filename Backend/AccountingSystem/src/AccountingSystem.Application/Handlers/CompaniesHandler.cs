// Application/Handlers/Companies/CompaniesHandler.cs
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application;
using AccountingSystem.Application.Queries.Companies;
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

    public Task<List<CompanyWithPendingReportsDto>> Handle(GetCompaniesByFirmIdQueryWithReport request, CancellationToken cancellationToken)
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

        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
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

        public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
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

        public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
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