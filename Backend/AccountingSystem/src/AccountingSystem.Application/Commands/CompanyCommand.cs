using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AccountingSystem.Application.Commands.Companies
{
    // ========================================
    // CREATE COMPANY
    // ========================================

    public class CreateCompanyCommand : IRequest<CompanyDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Taxid { get; set; } = string.Empty;
        public int Firmid { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public bool Isactive { get; set; } = true;
    }

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
            if (await _unitOfWork.Companies.TaxIdExistsAsync(request.Taxid))
            {
                throw new Exception($"חברה עם ח.פ/ע.מ {request.Taxid} כבר קיימת במערכת");
            }

            if (!await _unitOfWork.AccountingFirms.ExistsAsync(request.Firmid))
            {
                throw new Exception($"משרד רואי חשבון עם ID {request.Firmid} לא נמצא");
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
                Isactive = request.Isactive,
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow
            };

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();

            var createdCompany = await _unitOfWork.Companies.GetByIdAsync(company.Id);
            return _mapper.Map<CompanyDto>(createdCompany);
        }
    }

    // ========================================
    // UPDATE COMPANY
    // ========================================

    public class UpdateCompanyCommand : IRequest<CompanyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public bool Isactive { get; set; }
    }

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
            company.Address = request.Address;
            company.Phone = request.Phone;
            company.Email = request.Email;
            company.Notes = request.Notes;
            company.Isactive = request.Isactive;
            company.Updatedat = DateTime.UtcNow;

            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync();

            var updatedCompany = await _unitOfWork.Companies.GetByIdAsync(company.Id);
            return _mapper.Map<CompanyDto>(updatedCompany);
        }
    }

    // ========================================
    // DELETE COMPANY
    // ========================================

    public class DeleteCompanyCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteCompanyCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async AccountingSystem.Domain.Entities.Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            if (company == null)
            {
                throw new Exception($"חברה עם ID {request.Id} לא נמצאה");
            }

            await _unitOfWork.Companies.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}