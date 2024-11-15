using FluentValidation;
using Shared.DTO;

namespace TeamProjectManager.Validators
{
    public class ProjectValidator : AbstractValidator<ProjectDto>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"It is necessary to inform the user id. 'Project'");
        }
    }
}
