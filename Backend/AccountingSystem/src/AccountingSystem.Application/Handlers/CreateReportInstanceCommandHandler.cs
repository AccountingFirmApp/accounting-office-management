using MediatR;
using AccountingSystem.Application.Commands;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces.Repositories;

namespace AccountingSystem.Application.Handlers
{
    //public class CreateReportInstanceCommandHandler
    //    : IRequestHandler<CreateReportInstanceCommand, ReportInstanceDto>
    //{
    //    private readonly IReportInstanceRepository _reportInstanceRepository;
    //    private readonly IUnitOfWork _unitOfWork;

    //    public CreateReportInstanceCommandHandler(
    //        IReportInstanceRepository reportInstanceRepository,
    //        IUnitOfWork unitOfWork)
    //    {
    //        _reportInstanceRepository = reportInstanceRepository;
    //        _unitOfWork = unitOfWork;
    //    }

        //    public async Task<ReportInstanceDto> Handle(
        //        CreateReportInstanceCommand request,
        //        CancellationToken cancellationToken)
        //    {
        //        // המרת PaymentMethod מ-string ל-Enum (אם קיים)
        //        PaymentMethod? paymentMethod = null;
        //        if (!string.IsNullOrEmpty(request.PaymentMethod) &&
        //            Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var parsedMethod))
        //        {
        //            paymentMethod = parsedMethod;
        //        }

        //        var reportInstance = new Reportinstance
        //        {
        //            Configid = request.ConfigId,
        //            Period = DateOnly.FromDateTime(request.Period), // DateTime -> DateOnly
        //            Amount = request.Amount,
        //            Status = ReportStatus.Pending, // ברירת מחדל
        //            PaymentMethod = paymentMethod,
        //            Receiptdate = request.ReceiptDate.HasValue
        //                ? DateOnly.FromDateTime(request.ReceiptDate.Value)
        //                : null,
        //            Comments = request.Comments,
        //            Createdat = DateTime.Now,
        //            Updatedat = DateTime.Now
        //        };

        //        await _reportInstanceRepository.AddAsync(reportInstance);
        //        await _unitOfWork.SaveChangesAsync();

        //        return new ReportInstanceDto
        //        {
        //            Id = reportInstance.Id,
        //            ConfigId = reportInstance.Configid,
        //            Period = reportInstance.Period.ToDateTime(TimeOnly.MinValue),
        //            Amount = reportInstance.Amount,
        //            Status = reportInstance.Status.ToString(),
        //            PaymentMethod = reportInstance.PaymentMethod?.ToString(),
        //            ReceiptDate = reportInstance.Receiptdate?.ToDateTime(TimeOnly.MinValue),
        //            Comments = reportInstance.Comments ?? string.Empty,
        //            CreatedAt = reportInstance.Createdat,
        //            UpdatedAt = reportInstance.Updatedat
        //        };
        //    }
        //}

        public class CreateReportInstanceCommandHandler
    : IRequestHandler<CreateReportInstanceCommand, ReportInstanceDto>
        {
            private readonly IReportInstanceRepository _reportInstanceRepository;
            private readonly ICompanyReportConfigRepository _configRepository;
            private readonly IUnitOfWork _unitOfWork;

            public CreateReportInstanceCommandHandler(
                IReportInstanceRepository reportInstanceRepository,
                ICompanyReportConfigRepository configRepository,
                IUnitOfWork unitOfWork)
            {
                _reportInstanceRepository = reportInstanceRepository;
                _configRepository = configRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<ReportInstanceDto> Handle(
                CreateReportInstanceCommand request,
                CancellationToken cancellationToken)
            {
                // 🔍 שלב 1: בדוק אם קיים Config עם הצירוף הזה
                var configs = await _configRepository.GetByCompanyIdAsync(request.CompanyId);
                var existingConfig = configs.FirstOrDefault(c =>
                    c.Reporttypeid == request.ReportTypeId);

                int configId;

                if (existingConfig != null)
                {
                    // ✅ Config קיים - השתמש בו
                    configId = existingConfig.Id;
                }
                else
                {
                    // 🆕 Config לא קיים - צור חדש
                    var newConfig = new Companyreportconfig
                    {
                        Companyid = request.CompanyId,
                        Reporttypeid = request.ReportTypeId,
                        Frequencyid = request.FrequencyId ?? 1, // ברירת מחדל: חודשי
                        Dayofmonth = null, // או ערך ברירת מחדל
                        Isactive = true,
                        Createdat = DateTime.UtcNow,
                        Updatedat = DateTime.UtcNow
                    };

                    await _configRepository.AddAsync(newConfig);
                    await _unitOfWork.SaveChangesAsync();

                    configId = newConfig.Id;
                }

                // 📝 שלב 2: צור את ה-ReportInstance עם ה-ConfigId
                PaymentMethod? paymentMethod = null;
                if (!string.IsNullOrEmpty(request.PaymentMethod) &&
                    Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var parsedMethod))
                {
                    paymentMethod = parsedMethod;
                }

                var reportInstance = new Reportinstance
                {
                    Configid = configId, // ✅ משתמש ב-Config שנמצא או נוצר
                    Period = DateOnly.FromDateTime(request.Period),
                    Amount = request.Amount,
                    Status = ReportStatus.Pending,
                    PaymentMethod = paymentMethod,
                    Receiptdate = request.ReceiptDate.HasValue
                        ? DateOnly.FromDateTime(request.ReceiptDate.Value)
                        : null,
                    Comments = request.Comments,
                    Createdat = DateTime.UtcNow,
                    Updatedat = DateTime.UtcNow
                };

                await _reportInstanceRepository.AddAsync(reportInstance);
                await _unitOfWork.SaveChangesAsync();

                return new ReportInstanceDto
                {
                    Id = reportInstance.Id,
                    ConfigId = reportInstance.Configid,
                    Period = reportInstance.Period.ToDateTime(TimeOnly.MinValue),
                    Amount = reportInstance.Amount,
                    Status = reportInstance.Status.ToString(),
                    PaymentMethod = reportInstance.PaymentMethod?.ToString(),
                    ReceiptDate = reportInstance.Receiptdate?.ToDateTime(TimeOnly.MinValue),
                    Comments = reportInstance.Comments ?? string.Empty,
                    CreatedAt = reportInstance.Createdat,
                    UpdatedAt = reportInstance.Updatedat
                };
            }
        }



        public class UpdateReportStatusCommandHandler
        : IRequestHandler<UpdateReportStatusCommand, bool>
    {
        private readonly IReportInstanceRepository _reportInstanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReportStatusCommandHandler(
            IReportInstanceRepository reportInstanceRepository,
            IUnitOfWork unitOfWork)
        {
            _reportInstanceRepository = reportInstanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateReportStatusCommand request,
            CancellationToken cancellationToken)
        {
            var report = await _reportInstanceRepository.GetByIdAsync(request.Id);

            if (report == null)
                return false;

            // המרת string ל-Enum
            if (Enum.TryParse<ReportStatus>(request.Status, out var status))
            {
                report.Status = status;
            }
            else
            {
                return false; // סטטוס לא תקין
            }

            report.Updatedat = DateTime.UtcNow;

            // עדכון אוטומטי של תאריכים
            if (status == ReportStatus.Reported && !report.Reporteddate.HasValue)
            {
                report.Reporteddate = DateOnly.FromDateTime(DateTime.Now);
            }
            else if (status == ReportStatus.Paid && !report.Paiddate.HasValue)
            {
                report.Paiddate = DateOnly.FromDateTime(DateTime.Now);
            }

            _reportInstanceRepository.UpdateAsync(report);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }





    public class UpdateReportPaymentCommandHandler
        : IRequestHandler<UpdateReportPaymentCommand, bool>
    {
        private readonly IReportInstanceRepository _reportInstanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReportPaymentCommandHandler(
            IReportInstanceRepository reportInstanceRepository,
            IUnitOfWork unitOfWork)
        {
            _reportInstanceRepository = reportInstanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateReportPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var report = await _reportInstanceRepository.GetByIdAsync(request.Id);

            if (report == null)
                return false;

            report.Amount = request.Amount;

            // המרת string ל-Enum
            if (Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var paymentMethod))
            {
                report.PaymentMethod = paymentMethod;
            }

            report.Paiddate = DateOnly.FromDateTime(request.PaidDate);
            report.Status = ReportStatus.Paid;
            report.Updatedat = DateTime.Now;

            _reportInstanceRepository.UpdateAsync(report);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

    public class UpdateReportInstanceCommandHandler
       : IRequestHandler<UpdateReportInstanceCommand, bool>
    {
        private readonly IReportInstanceRepository _reportInstanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReportInstanceCommandHandler(
            IReportInstanceRepository reportInstanceRepository,
            IUnitOfWork unitOfWork)
        {
            _reportInstanceRepository = reportInstanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateReportInstanceCommand request,
            CancellationToken cancellationToken)
        {
            var report = await _reportInstanceRepository.GetByIdAsync(request.Id);

            if (report == null)
                return false;

            // עדכון השדות
            report.Amount = request.Amount;

            // המרת סטטוס
            if (Enum.TryParse<ReportStatus>(request.Status, out var status))
            {
                report.Status = status;
            }

            // המרת אמצעי תשלום
            if (!string.IsNullOrEmpty(request.PaymentMethod) &&
                Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var paymentMethod))
            {
                report.PaymentMethod = paymentMethod;
            }

            report.Receiptdate = request.ReceiptDate.HasValue
                ? DateOnly.FromDateTime(request.ReceiptDate.Value)
                : null;

            report.Reporteddate = request.ReportedDate.HasValue
                ? DateOnly.FromDateTime(request.ReportedDate.Value)
                : null;

            report.Paiddate = request.PaidDate.HasValue
                ? DateOnly.FromDateTime(request.PaidDate.Value)
                : null;

            report.Comments = request.Comments;
            report.Updatedat = DateTime.Now;

            _reportInstanceRepository.UpdateAsync(report);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}



