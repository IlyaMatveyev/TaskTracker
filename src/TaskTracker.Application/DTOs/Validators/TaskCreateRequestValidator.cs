using FluentValidation;

namespace TaskTracker.Application.DTOs.Validators
{
	public class TaskCreateRequestValidator : AbstractValidator<TaskCreateRequest>
	{
		public TaskCreateRequestValidator()
		{
			RuleFor(t => t.Title)
				.NotEmpty()
				.MinimumLength(Task.MIN_TITLE_LENGTH)
				.MaximumLength(Task.MAX_TITLE_LENGTH);

			RuleFor(t => t.Description)
				.MaximumLength(Task.MAX_DESCRIPTION_LENGTH);

			RuleFor(t => t.ProjectId)
				.NotEmpty();
		}
	}
}
