//using AccountingSystem.Application.DTOs;
//using FluentValidation;

//namespace AccountingSystem.Application.Validators.ReportInstance
//{
//    public class CreateReportInstanceDtoValidator : AbstractValidator<CreateReportInstanceDto>
//    {
//        public CreateReportInstanceDtoValidator()
//        {
//            // Config - חובה
//            RuleFor(x => x.ConfigId)
//                .NotEmpty().WithMessage("חובה לבחור הגדרת דיווח")
//                .GreaterThan(0).WithMessage("הגדרת דיווח לא תקינה");

//            // תקופה - חובה
//            RuleFor(x => x.Period)
//                .NotEmpty().WithMessage("תקופת הדיווח היא שדה חובה");

//            // סכום - אופציונלי, אבל אם קיים צריך להיות חיובי
//            RuleFor(x => x.Amount)
//                .GreaterThanOrEqualTo(0).WithMessage("סכום לא יכול להיות שלילי")
//                .When(x => x.Amount.HasValue);

//            // סטטוס - חובה ותקין


//            // אמצעי תשלום - אופציונלי אבל אם קיים צריך להיות תקין
//            RuleFor(x => x.PaymentMethod)
//                .Must(BeValidPaymentMethod).WithMessage("אמצעי תשלום לא תקין")
//                .When(x => !string.IsNullOrEmpty(x.PaymentMethod));





//            // הערות
//            RuleFor(x => x.Comments)
//                .MaximumLength(2000).WithMessage("הערות לא יכולות להיות יותר מ-2000 תווים")
//                .When(x => !string.IsNullOrEmpty(x.Comments));
//        }

//        // ולידציה מותאמת - סטטוס
//        private bool BeValidStatus(string status)
//        {
//            var validStatuses = new[] { "Pending", "Reported", "Paid", "Approved", "NotRequired" };
//            return validStatuses.Contains(status);
//        }

//        // ולידציה מותאמת - אמצעי תשלום
//        private bool BeValidPaymentMethod(string paymentMethod)
//        {
//            var validMethods = new[] { "Credit", "Transfer", "Check", "Online", "Cash" };
//            return validMethods.Contains(paymentMethod);
//        }
//    }
//}


using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.ReportInstance
{
    public class CreateReportInstanceDtoValidator : AbstractValidator<CreateReportInstanceDto>
    {
        public CreateReportInstanceDtoValidator()
        {
            // 🆕 Company - חובה
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("חובה לבחור חברה")
                .GreaterThan(0).WithMessage("חברה לא תקינה");

            // 🆕 ReportType - חובה
            RuleFor(x => x.ReportTypeId)
                .NotEmpty().WithMessage("חובה לבחור סוג דיווח")
                .GreaterThan(0).WithMessage("סוג דיווח לא תקין");

            // 🆕 Frequency - אופציונלי, אבל אם קיים צריך להיות תקין
            RuleFor(x => x.FrequencyId)
                .GreaterThan(0).WithMessage("תדירות לא תקינה")
                .When(x => x.FrequencyId.HasValue);

            // תקופה - חובה
            RuleFor(x => x.Period)
                .NotEmpty().WithMessage("תקופת הדיווח היא שדה חובה");

            // סכום - אופציונלי, אבל אם קיים צריך להיות חיובי
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("סכום לא יכול להיות שלילי")
                .When(x => x.Amount.HasValue);

            // אמצעי תשלום - אופציונלי אבל אם קיים צריך להיות תקין
            RuleFor(x => x.PaymentMethod)
                .Must(BeValidPaymentMethod).WithMessage("אמצעי תשלום לא תקין")
                .When(x => !string.IsNullOrEmpty(x.PaymentMethod));

            // הערות
            RuleFor(x => x.Comments)
                .MaximumLength(2000).WithMessage("הערות לא יכולות להיות יותר מ-2000 תווים")
                .When(x => !string.IsNullOrEmpty(x.Comments));
        }

        // ולידציה מותאמת - אמצעי תשלום
        private bool BeValidPaymentMethod(string paymentMethod)
        {
            var validMethods = new[] { "Credit", "Transfer", "Check", "Online", "Cash" };
            return validMethods.Contains(paymentMethod);
        }
    }
}