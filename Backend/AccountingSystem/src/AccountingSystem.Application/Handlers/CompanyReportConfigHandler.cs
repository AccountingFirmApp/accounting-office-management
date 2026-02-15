using AccountingSystem.Application.Commands;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries.Tasks;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Handlers
{
    public class CreateCompanyReportConfigHandler : IRequestHandler<CreateCompanyConfigReportCommand, CompanyReportConfigDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCompanyReportConfigHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CompanyReportConfigDto> Handle(CreateCompanyConfigReportCommand request, CancellationToken cancellationToken)
        {
            var company =await _unitOfWork.Companies.ExistsAsync(request.CompanyId);
            if (!company)
            {
                throw new Exception($"חברה עם ID {request.CompanyId} לא נמצאה");

            }
            var freq = await _unitOfWork.Frequencies.ExistsAsync(request.FrequencyId);
            if (!freq)
            {
                throw new Exception($"תדירות עם ID {request.FrequencyId} לא נמצאה");

            }
            var reportTypeId = await _unitOfWork.ReportTypes.ExistsAsync(request.ReportTypeId);
            if (!reportTypeId)
            {
                throw new Exception($"תדירות עם ID {request.ReportTypeId} לא נמצאה");

            }
            //בדיקה אם לא קיים כזה דיווח לחברה בשנה זו
                var existingConfig = await _unitOfWork.CompanyReportConfigs
                 .FindAsync(c =>
                    c.Companyid == request.CompanyId &&
                    c.Reporttypeid == request.ReportTypeId &&
                    c.Year == request.Year);

            if (existingConfig.Any())
            {
                throw new InvalidOperationException("כבר קיימת הגדרה זהה");
            }
            var reportConfig = new Companyreportconfig()
            {
                Companyid = request.CompanyId,
                Reporttypeid = request.ReportTypeId,
                Frequencyid = request.FrequencyId,
                Isactive = true,
                Year = request.Year,
                Createdat = DateTime.UtcNow, 
                Updatedat = DateTime.UtcNow
            };
          await _unitOfWork.CompanyReportConfigs.AddAsync(reportConfig);
            await _unitOfWork.SaveChangesAsync();
            var configWithDetails = await _unitOfWork.CompanyReportConfigs
                .GetByIdWithDetailsAsync(reportConfig.Id);
            if (configWithDetails == null)
            {
                throw new InvalidOperationException("שגיאה בשליפת ההגדרה שנוצרה");
            }

            return _mapper.Map<CompanyReportConfigDto>(configWithDetails);
        }


        public class DeleteompanyReportConfigHandler : IRequestHandler<DeleteCompanyConfigReportCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public DeleteompanyReportConfigHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<bool> Handle(DeleteCompanyConfigReportCommand request, CancellationToken cancellationToken)
            {
                var config = await _unitOfWork.CompanyReportConfigs.GetByIdAsync(request.Id);
                if (config == null)
                {
                    throw new Exception($"הגדרה עם ID {request.Id} לא נמצאה");

                }
                config.Isactive = false;
                _unitOfWork.SaveChangesAsync();

                return true;
            }

        }
        }
    }
