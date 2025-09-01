using FluentValidation;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.DTOs.Validators
{
	public class ProjectUpdateRequestValidator : AbstractValidator<ProjectUpdateRequest>
	{
		public ProjectUpdateRequestValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty()
				.MaximumLength(Project.MAX_NAME_LENGTH);

			RuleFor(p => p.Description)
				.MaximumLength(Project.MAX_DESCRIPTION_LENGTH);
		}
	}
}
