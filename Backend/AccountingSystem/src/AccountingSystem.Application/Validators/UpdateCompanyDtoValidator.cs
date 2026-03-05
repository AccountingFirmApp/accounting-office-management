using AccountingSystem.Application.DTOs;
using FluentValidation;

namespace AccountingSystem.Application.Validators.Company
{
    /// <summary>
    /// ולידציה לעדכון חברה קיימת
    /// </summary>
    public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID הוא שדה חובה")
                .GreaterThan(0).WithMessage("ID לא תקין");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("שם החברה הוא שדה חובה")
                .MaximumLength(255).WithMessage("שם החברה לא יכול להיות יותר מ-255 תווים")
                .MinimumLength(2).WithMessage("שם החברה חייב להכיל לפחות 2 תווים");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("פורמט אימייל לא תקין")
                .MaximumLength(255).WithMessage("אימייל לא יכול להיות יותר מ-255 תווים")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(50).WithMessage("מספר טלפון לא יכול להיות יותר מ-50 תווים")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("כתובת לא יכולה להיות יותר מ-500 תווים")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("הערות לא יכולות להיות יותר מ-2000 תווים")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }
}