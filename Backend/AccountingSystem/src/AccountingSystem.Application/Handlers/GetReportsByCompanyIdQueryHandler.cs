
using MediatR;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AccountingSystem.Application.Handlers
{
    public class GetReportsByCompanyIdQueryHandler
        : IRequestHandler<GetReportsByCompanyIdQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _reportInstanceRepository;

        public GetReportsByCompanyIdQueryHandler(IReportInstanceRepository reportInstanceRepository)
        {
            _reportInstanceRepository = reportInstanceRepository;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(
            GetReportsByCompanyIdQuery request,
            CancellationToken cancellationToken)
        {
            var reports = await _reportInstanceRepository.GetAllAsync();

            var filteredReports = reports
                .Where(r => r.Config != null && r.Config.Companyid == request.CompanyId);

            if (!string.IsNullOrEmpty(request.Status))
            {
                filteredReports = filteredReports.Where(r => r.Status.ToString() == request.Status);
            }

            if (request.FromPeriod.HasValue)
            {
                var fromDateOnly = DateOnly.FromDateTime(request.FromPeriod.Value);
                filteredReports = filteredReports.Where(r => r.Period >= fromDateOnly);
            }

            if (request.ToPeriod.HasValue)
            {
                var toDateOnly = DateOnly.FromDateTime(request.ToPeriod.Value);
                filteredReports = filteredReports.Where(r => r.Period <= toDateOnly);
            }

            var result = filteredReports
                .OrderByDescending(r => r.Period)
                .ToList()
                .Select(r => new ReportInstanceDetailDto
                {
                    Id = r.Id,
                    ConfigId = r.Configid,

                    CompanyId = r.Config.Companyid,
                    CompanyName = r.Config.Company != null ? r.Config.Company.Name : string.Empty,
                    CompanyTaxId = r.Config.Company != null ? r.Config.Company.Taxid : string.Empty,

                    ReportTypeId = r.Config.Reporttypeid,
                    ReportTypeName = r.Config.Reporttype != null ? r.Config.Reporttype.Name : string.Empty,
                    ReportTypeShortCode = r.Config.Reporttype != null ? r.Config.Reporttype.Shortcode : string.Empty,

                    FrequencyName = r.Config.Frequency != null ? r.Config.Frequency.Name : string.Empty,
                    DayOfMonth = r.Config.Dayofmonth,

                    Period = r.Period.ToDateTime(TimeOnly.MinValue),
                    Amount = r.Amount,
                    Status = r.Status.ToString(),
                    PaymentMethod = r.PaymentMethod.HasValue ? r.PaymentMethod.Value.ToString() : null,
                    ReceiptDate = r.Receiptdate.HasValue ? r.Receiptdate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    ReportedDate = r.Reporteddate.HasValue ? r.Reporteddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    PaidDate = r.Paiddate.HasValue ? r.Paiddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    Comments = r.Comments ?? string.Empty
                })
                .ToList();

            return result;
        }
    }



    public class GetReportByIdQueryHandler
        : IRequestHandler<GetReportByIdQuery, ReportInstanceDetailDto?>
    {
        private readonly IReportInstanceRepository _reportInstanceRepository;

        public GetReportByIdQueryHandler(IReportInstanceRepository reportInstanceRepository)
        {
            _reportInstanceRepository = reportInstanceRepository;
        }

        public async Task<ReportInstanceDetailDto?> Handle(
            GetReportByIdQuery request,
            CancellationToken cancellationToken)
        {
            var report = await _reportInstanceRepository.GetByIdAsync(request.ReportId);

            if (report == null)
                return null;

            return new ReportInstanceDetailDto
            {
                Id = report.Id,
                ConfigId = report.Configid,

                CompanyId = report.Config.Companyid,
                CompanyName = report.Config.Company != null ? report.Config.Company.Name : string.Empty,
                CompanyTaxId = report.Config.Company != null ? report.Config.Company.Taxid : string.Empty,

                ReportTypeId = report.Config.Reporttypeid,
                ReportTypeName = report.Config.Reporttype != null ? report.Config.Reporttype.Name : string.Empty,
                ReportTypeShortCode = report.Config.Reporttype != null ? report.Config.Reporttype.Shortcode : string.Empty,

                FrequencyName = report.Config.Frequency != null ? report.Config.Frequency.Name : string.Empty,
                DayOfMonth = report.Config.Dayofmonth,

                Period = report.Period.ToDateTime(TimeOnly.MinValue),
                Amount = report.Amount,
                Status = report.Status.ToString(),
                PaymentMethod = report.PaymentMethod.HasValue ? report.PaymentMethod.Value.ToString() : null,
                ReceiptDate = report.Receiptdate.HasValue ? report.Receiptdate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                ReportedDate = report.Reporteddate.HasValue ? report.Reporteddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                PaidDate = report.Paiddate.HasValue ? report.Paiddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                Comments = report.Comments ?? string.Empty
            };
        }
    }



    public class GetUpcomingReportsQueryHandler
       : IRequestHandler<GetUpcomingReportsQuery, List<UpcomingReportDto>>
    {
        private readonly IReportInstanceRepository _reportInstanceRepository;

        public GetUpcomingReportsQueryHandler(IReportInstanceRepository reportInstanceRepository)
        {
            _reportInstanceRepository = reportInstanceRepository;
        }

        public async Task<List<UpcomingReportDto>> Handle(
            GetUpcomingReportsQuery request,
            CancellationToken cancellationToken)
        {
            var allReports = await _reportInstanceRepository.GetAllAsync();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var futureDate = today.AddDays(request.DaysAhead);

            var upcomingReports = allReports
                .Where(r =>
                    (r.Status == ReportStatus.Pending || r.Status == ReportStatus.Reported) &&
                    r.Period <= futureDate);

            if (request.CompanyId.HasValue)
            {
                upcomingReports = upcomingReports
                    .Where(r => r.Config.Companyid == request.CompanyId.Value);
            }

            var result = upcomingReports
                .OrderBy(r => r.Period)
                .ToList() 
                .Select(r => new UpcomingReportDto
                {
                    Id = r.Id,
                    CompanyName = r.Config.Company != null ? r.Config.Company.Name : string.Empty,
                    ReportTypeName = r.Config.Reporttype != null ? r.Config.Reporttype.Name : string.Empty,
                    ShortCode = r.Config.Reporttype != null ? r.Config.Reporttype.Shortcode : string.Empty,
                    Period = r.Period.ToDateTime(TimeOnly.MinValue),
                    Status = r.Status.ToString(),
                    Amount = r.Amount,
                    DayOfMonth = r.Config.Dayofmonth,
                    DaysOverdue = r.Period < today ? (today.DayNumber - r.Period.DayNumber) : 0
                })
                .ToList();

            return result;
        }
    }

   
    public class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly ICompanyWorkerRepository _companyWorkerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllReportsQueryHandler> _logger;

        public GetAllReportsQueryHandler(
            IReportInstanceRepository repository,
            ICompanyWorkerRepository companyWorkerRepository,
            IMapper mapper,
            ILogger<GetAllReportsQueryHandler>logger)
        {
            _repository = repository;
            _companyWorkerRepository = companyWorkerRepository;
            _mapper = mapper;
            _logger= logger;
        }

                  public async Task<List<ReportInstanceDetailDto>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"📋 Handler: IsAdminMode={request.IsAdminMode}, WorkerId={request.WorkerId}");

                var reports = await _repository.GetAllAsync();

                if (request.WorkerId.HasValue)
                {
                    _logger.LogInformation($"🔍 מסנן לפי WorkerId={request.WorkerId.Value}");

                    reports = reports.Where(r =>
                        r.Config != null &&
                        r.Config.Company != null &&
                        r.Config.Company.Companyworkers.Any(cw =>
                            cw.Workerid == request.WorkerId.Value &&
                            cw.Isactive == true
                        )
                    ).ToList();
                }

                _logger.LogInformation($" אחרי פילטור: {reports.Count()} דוחות");

                var mappedReports = _mapper.Map<List<ReportInstanceDetailDto>>(
                    reports,
                    opt => opt.Items["IsAdminMode"] = request.IsAdminMode
                );

                _logger.LogInformation($" החזרת {mappedReports.Count} דוחות");

                return mappedReports;
            }
        }
    }

    // ========== Handler 2: דיווחים לפי Config ==========

    public class GetReportsByConfigIdQueryHandler : IRequestHandler<GetReportsByConfigIdQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetReportsByConfigIdQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetReportsByConfigIdQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetReportsByConfigIdAsync(request.ConfigId);
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }
    }


    public class GetReportsByStatusQueryHandler : IRequestHandler<GetReportsByStatusQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetReportsByStatusQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetReportsByStatusQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetReportsByStatusAsync(request.Status);
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }
    }


    public class GetPendingReportsQueryHandler : IRequestHandler<GetPendingReportsQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetPendingReportsQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetPendingReportsQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetPendingReportsAsync();
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }
    }


    public class GetReportsByPeriodQueryHandler : IRequestHandler<GetReportsByPeriodQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetReportsByPeriodQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetReportsByPeriodQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetReportsByPeriodAsync(request.Period);
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }
    }


    public class GetReportsByDateRangeQueryHandler : IRequestHandler<GetReportsByDateRangeQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetReportsByDateRangeQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetReportsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetReportsByDateRangeAsync(request.StartDate, request.EndDate);
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }
    }


    public class GetOverdueReportsQueryHandler : IRequestHandler<GetOverdueReportsQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetOverdueReportsQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetOverdueReportsQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetOverdueReportsAsync();
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }
    }


    public class GetReportsDueInNextDaysQueryHandler : IRequestHandler<GetReportsDueInNextDaysQuery, List<ReportInstanceDetailDto>>
    {
        private readonly IReportInstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetReportsDueInNextDaysQueryHandler(IReportInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportInstanceDetailDto>> Handle(GetReportsDueInNextDaysQuery request, CancellationToken cancellationToken)
        {
            var reports = await _repository.GetReportsDueInNextDaysAsync(request.Days);
            return _mapper.Map<List<ReportInstanceDetailDto>>(reports);
        }

        // ========== Report Types Handlers ==========

        public class GetAllReportTypesQueryHandler : IRequestHandler<GetAllReportTypesQuery, List<ReportTypeDto>>
        {
            private readonly IReportTypeRepository _repository;
            private readonly IMapper _mapper;

            public GetAllReportTypesQueryHandler(IReportTypeRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<ReportTypeDto>> Handle(GetAllReportTypesQuery request, CancellationToken cancellationToken)
            {
                var reportTypes = await _repository.GetAllAsync();
                return _mapper.Map<List<ReportTypeDto>>(reportTypes);
            }
        }
    public class GetReportTypesToEditQueryHandler : IRequestHandler<GetAllReportTypesToEditQuery, List<ReportTypeDto>>
    {
        private readonly IReportTypeRepository _repository;
        private readonly IMapper _mapper;

    

        public GetReportTypesToEditQueryHandler(IReportTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportTypeDto>> Handle(GetAllReportTypesToEditQuery request, CancellationToken cancellationToken)
        {
            var reportTypes = await _repository.GetToEdit();
            return _mapper.Map<List<ReportTypeDto>>(reportTypes);
        }
    }
    public class GetReportTypeByIdQueryHandler : IRequestHandler<GetReportTypeByIdQuery, ReportTypeDto?>
        {
            private readonly IReportTypeRepository _repository;
            private readonly IMapper _mapper;

            public GetReportTypeByIdQueryHandler(IReportTypeRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ReportTypeDto?> Handle(GetReportTypeByIdQuery request, CancellationToken cancellationToken)
            {
                var reportType = await _repository.GetByIdAsync(request.Id);
                return reportType != null ? _mapper.Map<ReportTypeDto>(reportType) : null;
            }
        }

        // ========== Company Report Configs Handlers ==========

        public class GetAllConfigsQueryHandler : IRequestHandler<GetAllConfigsQuery, List<CompanyReportConfigDto>>
        {
            private readonly ICompanyreportconfigRepository _repository;
            private readonly IMapper _mapper;

            public GetAllConfigsQueryHandler(ICompanyreportconfigRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<CompanyReportConfigDto>> Handle(GetAllConfigsQuery request, CancellationToken cancellationToken)
            {
                var configs = await _repository.GetAllAsync();

                return configs.Select(c => new CompanyReportConfigDto
                {
                    Id = c.Id,
                    CompanyId = c.Companyid,
                    CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                    ReportTypeId = c.Reporttypeid,
                    ReportTypeName = c.Reporttype != null ? c.Reporttype.Name : string.Empty,
                    FrequencyId = c.Frequencyid,
                    FrequencyName = c.Frequency != null ? c.Frequency.Name : string.Empty,
                    DayOfMonth = c.Dayofmonth,
                    Isactive = c.Isactive ?? true,
                    Year=c.Year
                }).ToList();
            }
        }

        public class GetConfigsByCompanyIdQueryHandler : IRequestHandler<GetConfigsByCompanyIdQuery, List<CompanyReportConfigDto>>
        {
            private readonly ICompanyreportconfigRepository _repository;

            public GetConfigsByCompanyIdQueryHandler(ICompanyreportconfigRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<CompanyReportConfigDto>> Handle(GetConfigsByCompanyIdQuery request, CancellationToken cancellationToken)
            {
                var configs = await _repository.GetByCompanyIdAsync(request.CompanyId);

                return configs.Select(c => new CompanyReportConfigDto
                {
                    Id = c.Id,
                    CompanyId = c.Companyid,
                    CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                    ReportTypeId = c.Reporttypeid,
                    ReportTypeName = c.Reporttype != null ? c.Reporttype.Name : string.Empty,
                    FrequencyId = c.Frequencyid,
                    FrequencyName = c.Frequency != null ? c.Frequency.Name : string.Empty,
                    DayOfMonth = c.Dayofmonth,
                    Isactive = c.Isactive
                }).ToList();
            }
        }

        public class GetConfigByIdQueryHandler : IRequestHandler<GetConfigByIdQuery, CompanyReportConfigDto?>
        {
            private readonly ICompanyreportconfigRepository _repository;

            public GetConfigByIdQueryHandler(ICompanyreportconfigRepository repository)
            {
                _repository = repository;
            }

            public async Task<CompanyReportConfigDto?> Handle(GetConfigByIdQuery request, CancellationToken cancellationToken)
            {
                var config = await _repository.GetByIdAsync(request.Id);

                if (config == null)
                    return null;

                return new CompanyReportConfigDto
                {
                    Id = config.Id,
                    CompanyId = config.Companyid,
                    CompanyName = config.Company != null ? config.Company.Name : string.Empty,
                    ReportTypeId = config.Reporttypeid,
                    ReportTypeName = config.Reporttype != null ? config.Reporttype.Name : string.Empty,
                    FrequencyId = config.Frequencyid,
                    FrequencyName = config.Frequency != null ? config.Frequency.Name : string.Empty,
                    DayOfMonth = config.Dayofmonth,
                    Isactive = config.Isactive ?? true
                };
            }
        }
    }
