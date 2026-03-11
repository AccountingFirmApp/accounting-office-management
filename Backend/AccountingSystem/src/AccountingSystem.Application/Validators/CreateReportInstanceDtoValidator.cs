


using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.ReportInstance
{
    public class CreateReportInstanceDtoValidator : AbstractValidator<CreateReportInstanceDto>
    {
        public CreateReportInstanceDtoValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("חובה לבחור חברה")
                .GreaterThan(0).WithMessage("חברה לא תקינה");

            RuleFor(x => x.ReportTypeId)
                .NotEmpty().WithMessage("חובה לבחור סוג דיווח")
                .GreaterThan(0).WithMessage("סוג דיווח לא תקין");

            RuleFor(x => x.FrequencyId)
                .GreaterThan(0).WithMessage("תדירות לא תקינה")
                .When(x => x.FrequencyId.HasValue);

            RuleFor(x => x.Period)
                .NotEmpty().WithMessage("תקופת הדיווח היא שדה חובה");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("סכום לא יכול להיות שלילי")
                .When(x => x.Amount.HasValue);

            RuleFor(x => x.PaymentMethod)
                .Must(BeValidPaymentMethod).WithMessage("אמצעי תשלום לא תקין")
                .When(x => !string.IsNullOrEmpty(x.PaymentMethod));

             
            RuleFor(x => x.Comments)
                .MaximumLength(2000).WithMessage("הערות לא יכולות להיות יותר מ-2000 תווים")
                .When(x => !string.IsNullOrEmpty(x.Comments));
        }

        private bool BeValidPaymentMethod(string paymentMethod)
        {
            var validMethods = new[] { "Credit", "Transfer", "Check", "Online", "Cash" };
            return validMethods.Contains(paymentMethod);
        }
    }
}