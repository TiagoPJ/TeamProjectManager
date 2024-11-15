using FluentValidation;
using Shared.DTO;

namespace TeamProjectManager.Validators
{
    public class TaskValidator : AbstractValidator<TaskDto>
    {
        public TaskValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform the project id. 'Task'");

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform the user id. 'Task'");

            RuleFor(x => x.ExpirationDate)
                .NotNull()
                .NotEmpty()
                .GreaterThan(x => DateTime.Now).WithMessage("The expiration date, needs to be greater than today. 'Task'");
        }
    }
}
