using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.Worker
{
    public class UpdateWorkerDtoValidator : AbstractValidator<UpdateWorkerDto>
    {
        public UpdateWorkerDtoValidator()
        {
            // ID - חובה
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID הוא שדה חובה")
                .GreaterThan(0).WithMessage("ID לא תקין");

            // שם פרטי
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("שם פרטי הוא שדה חובה")
                .MaximumLength(100).WithMessage("שם פרטי לא יכול להיות יותר מ-100 תווים");

            // שם משפחה
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("שם משפחה הוא שדה חובה")
                .MaximumLength(100).WithMessage("שם משפחה לא יכול להיות יותר מ-100 תווים");

            // אימייל
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("אימייל הוא שדה חובה")
                .EmailAddress().WithMessage("פורמט אימייל לא תקין")
                .MaximumLength(255).WithMessage("אימייל לא יכול להיות יותר מ-255 תווים");

            // טלפון
            RuleFor(x => x.Phone)
                .MaximumLength(50).WithMessage("מספר טלפון לא יכול להיות יותר מ-50 תווים")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }
}