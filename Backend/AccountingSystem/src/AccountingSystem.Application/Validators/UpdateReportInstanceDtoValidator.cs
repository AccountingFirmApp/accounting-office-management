using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.ReportInstance
{
    public class UpdateReportInstanceDtoValidator : AbstractValidator<UpdateReportInstanceDto>
    {
        public UpdateReportInstanceDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID הוא שדה חובה")
                .GreaterThan(0).WithMessage("ID לא תקין");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("סכום לא יכול להיות שלילי")
                .When(x => x.Amount.HasValue);

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("סטטוס הוא שדה חובה")
                .Must(BeValidStatus).WithMessage("סטטוס לא תקין");

            RuleFor(x => x.PaymentMethod)
                .Must(BeValidPaymentMethod).WithMessage("אמצעי תשלום לא תקין")
                .When(x => !string.IsNullOrEmpty(x.PaymentMethod));

            RuleFor(x => x.Comments)
                .MaximumLength(2000).WithMessage("הערות לא יכולות להיות יותר מ-2000 תווים")
                .When(x => !string.IsNullOrEmpty(x.Comments));
        }

        private bool BeValidStatus(string status)
        {
            var validStatuses = new[] { "Pending", "Reported", "Paid", "Approved", "NotRequired" };
            return validStatuses.Contains(status);
        }

        private bool BeValidPaymentMethod(string paymentMethod)
        {
            var validMethods = new[] { "Credit", "Transfer", "Check", "Online", "Cash" };
            return validMethods.Contains(paymentMethod);
        }
    }
}