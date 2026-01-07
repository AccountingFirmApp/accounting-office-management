using AccountingSystem.Application.Queries.Workers;
using FluentValidation;

namespace AccountingSystem.Application.Validators.Workers;

public class GetWorkerCompaniesValidator : AbstractValidator<GetWorkerCompaniesQuery>
{
    public GetWorkerCompaniesValidator()
    {
        RuleFor(x => x.WorkerId)
            .GreaterThan(0)
            .WithMessage("מזהה עובדת חייב להיות גדול מ-0");
    }
}