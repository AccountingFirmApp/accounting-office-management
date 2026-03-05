using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.Worker
{
    public class CreateWorkerDtoValidator : AbstractValidator<CreateWorkerDto>
    {
        public CreateWorkerDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("שם פרטי הוא שדה חובה")
                .MaximumLength(100).WithMessage("שם פרטי לא יכול להיות יותר מ-100 תווים")
                .MinimumLength(2).WithMessage("שם פרטי חייב להכיל לפחות 2 תווים");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("שם משפחה הוא שדה חובה")
                .MaximumLength(100).WithMessage("שם משפחה לא יכול להיות יותר מ-100 תווים")
                .MinimumLength(2).WithMessage("שם משפחה חייב להכיל לפחות 2 תווים");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("אימייל הוא שדה חובה")
                .EmailAddress().WithMessage("פורמט אימייל לא תקין")
                .MaximumLength(255).WithMessage("אימייל לא יכול להיות יותר מ-255 תווים");

            RuleFor(x => x.Phone)
                .MaximumLength(50).WithMessage("מספר טלפון לא יכול להיות יותר מ-50 תווים")
                .Matches(@"^[\d\-\+\(\)\s]+$").WithMessage("מספר טלפון מכיל תווים לא חוקיים")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.FirmId)
                .NotEmpty().WithMessage("חובה לבחור משרד רואי חשבון")
                .GreaterThan(0).WithMessage("משרד רואי חשבון לא תקין");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("חובה לבחור תפקיד")
                .GreaterThan(0).WithMessage("תפקיד לא תקין");

            // מספר עובד - אופציונלי
            RuleFor(x => x.Employeeid)
                .MaximumLength(50).WithMessage("מספר עובד לא יכול להיות יותר מ-50 תווים")
                .When(x => !string.IsNullOrEmpty(x.Employeeid));
        }
    }
}