using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorProject : AbstractValidator<Project>
{
    public ValidatorProject()
    {
        RuleFor(r => r.UserId)
            .GreaterThan(0).WithMessage(MessageConst.UserIdRequired);

        RuleFor(m => m.Title)
            .NotNull().NotEmpty().WithName(MessageConst.TitleRequired)
            .MaximumLength(100).WithMessage(MessageConst.TitleMaxPermitted);

        RuleFor(m => m.Description)
            .NotNull().NotEmpty().WithName(MessageConst.DescriptionRequired)
            .MaximumLength(150).WithMessage(MessageConst.DescriptionMaxPermitted);

        RuleFor(r => r.ExpectedStartDate)
            .NotNull().WithMessage(MessageConst.ExpectedStartDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.ExpectedStartDateRequired);

        RuleFor(r => r.ExpectedEndDate)
            .NotNull().WithMessage(MessageConst.ExpectedEndDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.ExpectedEndDateRequired);
    }
}