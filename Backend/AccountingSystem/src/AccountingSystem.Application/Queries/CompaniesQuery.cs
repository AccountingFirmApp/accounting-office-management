using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AccountingSystem.Application.Queries.Companies
{
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
    // GET ALL COMPANIES
    // ========================================

    public class GetAllCompaniesQuery : IRequest<List<CompanyDto>>
    {
    }

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
            return _mapper.Map<List<CompanyDto>>(companies.ToList());
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
            return _mapper.Map<List<CompanyDto>>(companies.ToList());
        }
    }
}