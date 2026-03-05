using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.Worker
{
    public class UpdateWorkerDtoValidator : AbstractValidator<UpdateWorkerDto>
    {
        public UpdateWorkerDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID הוא שדה חובה")
                .GreaterThan(0).WithMessage("ID לא תקין");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("שם פרטי הוא שדה חובה")
                .MaximumLength(100).WithMessage("שם פרטי לא יכול להיות יותר מ-100 תווים");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("שם משפחה הוא שדה חובה")
                .MaximumLength(100).WithMessage("שם משפחה לא יכול להיות יותר מ-100 תווים");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("אימייל הוא שדה חובה")
                .EmailAddress().WithMessage("פורמט אימייל לא תקין")
                .MaximumLength(255).WithMessage("אימייל לא יכול להיות יותר מ-255 תווים");

            RuleFor(x => x.Phone)
                .MaximumLength(50).WithMessage("מספר טלפון לא יכול להיות יותר מ-50 תווים")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }
}