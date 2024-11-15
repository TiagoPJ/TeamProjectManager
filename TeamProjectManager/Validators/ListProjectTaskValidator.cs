using Entities.Entities;
using FluentValidation;

namespace TeamProjectManager.Validators
{
    public class ListProjectTaskValidator : AbstractValidator<IEnumerable<ProjectTask>>
    {
        public ListProjectTaskValidator()
        {
            RuleForEach(listTask => listTask)
            .ChildRules(task =>
            {
                task.RuleFor(x => x)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage($"It is necessary to inform the user id. 'Task'");

                task.RuleFor(x => x.ProjectId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage($"It is necessary to inform the project id. 'Task'");

                task.RuleFor(x => x.Title)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage($"It is necessary to inform a title. 'Task'");

                task.RuleFor(x => x.Description)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage($"It is necessary to inform a description. 'Task'");
            });
        }
    }
}
