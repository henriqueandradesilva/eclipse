using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorTaskComment : AbstractValidator<TaskComment>
{
    public ValidatorTaskComment()
    {
        RuleFor(r => r.TaskId)
            .GreaterThan(0).WithMessage(MessageConst.ProjectIdRequired);

        RuleFor(r => r.UserId)
            .GreaterThan(0).WithMessage(MessageConst.UserIdRequired);

        RuleFor(m => m.Description)
           .NotNull().NotEmpty().WithName(MessageConst.DescriptionRequired)
           .MaximumLength(150).WithMessage(MessageConst.DescriptionMaxPermitted);
    }
}