using AccountingSystem.Application.Commands;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
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
            var companyExists = await _unitOfWork.Companies.ExistsAsync(request.CompanyId);
            if (!companyExists)
                throw new Exception($"חברה עם ID {request.CompanyId} לא נמצאה");

            var freqExists = await _unitOfWork.Frequencies.ExistsAsync(request.FrequencyId);
            if (!freqExists)
                throw new Exception($"תדירות עם ID {request.FrequencyId} לא נמצאה");

            var reportType = await _unitOfWork.ReportTypes.GetByIdAsync(request.ReportTypeId);
            if (reportType == null)
                throw new Exception($"ReportType עם ID {request.ReportTypeId} לא נמצא");

            // בדיקה אם כבר קיימת הגדרה זהה
            var existingConfig = await _unitOfWork.CompanyReportConfigs
                .FindAsync(c =>
                    c.Companyid == request.CompanyId &&
                    c.Reporttypeid == request.ReportTypeId &&
                    c.Year == request.Year);

            if (existingConfig.Any())
                throw new InvalidOperationException("כבר קיימת הגדרה זהה");

            // יצירת CompanyReportConfig עם DayOfMonth ברירת מחדל מה-ReportType אם לא נשלח
            var reportConfig = new Companyreportconfig()
            {
                Companyid = request.CompanyId,
                Reporttypeid = request.ReportTypeId,
                Frequencyid = request.FrequencyId,
                Dayofmonth = request.DayOfMonth ?? reportType.DefaultDayOfMonth,
                Isactive = true,
                Year = request.Year,
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow
            };

            await _unitOfWork.CompanyReportConfigs.AddAsync(reportConfig);
            await _unitOfWork.SaveChangesAsync();

            var configWithDetails = await _unitOfWork.CompanyReportConfigs
          .FindAsync(c => c.Id == reportConfig.Id);

            if (!configWithDetails.Any())
                throw new InvalidOperationException("שגיאה בשליפת ההגדרה שנוצרה");

            return _mapper.Map<CompanyReportConfigDto>(configWithDetails.First());
        }
    }
    public class DeleteCompanyReportConfigHandler
    : IRequestHandler<DeleteCompanyConfigReportCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteCompanyReportConfigHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            DeleteCompanyConfigReportCommand request,
            CancellationToken cancellationToken)
        {
            var config = await _unitOfWork
                .CompanyReportConfigs
                .GetByIdAsync(request.Id);

            if (config == null)
            {
                throw new Exception($"הגדרה עם ID {request.Id} לא נמצאה");
            }

            config.Isactive = false;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }


    public class GetConfigsByWorkerIdHandler : IRequestHandler<GetConfigsByWorkerId, List<CompanyReportConfigDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetConfigsByWorkerIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<List<CompanyReportConfigDto>> Handle(GetConfigsByWorkerId request, CancellationToken cancellationToken)
        {
            bool isExs = await _unitOfWork.Workers.ExistsAsync(request.WorkerId);
            if (!isExs)
            {
                throw new Exception( "the workerId not found");
            }
            List<Companyreportconfig> list = await _unitOfWork.CompanyReportConfigs.GetByWorkerId(request.WorkerId);
            return _mapper.Map<List<CompanyReportConfigDto>>(list);
        }
    }
    }
