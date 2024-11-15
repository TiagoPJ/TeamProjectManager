using Entities.Entities;
using FluentValidation;

namespace TeamProjectManager.Validators
{
    public class ProjectTaskValidator : AbstractValidator<ProjectTask>
    {
        public ProjectTaskValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform the user id. 'Task'");

            RuleFor(x => x.ProjectId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform the project id. 'Task'");

            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform a title. 'Task'");

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform a description. 'Task'");
        }
    }
}
