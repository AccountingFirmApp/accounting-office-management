
using AccountingSystem.Application.Commands.Workers;
using FluentValidation;

namespace AccountingSystem.Application.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("אימייל הוא שדה חובה")
            .EmailAddress().WithMessage("אימייל לא תקין")
            .MaximumLength(255).WithMessage("אימייל ארוך מדי");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("סיסמה היא שדה חובה")
            .MinimumLength(6).WithMessage("הסיסמה חייבת להכיל לפחות 6 תווים");
    }
}