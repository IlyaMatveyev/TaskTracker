using FluentValidation;
using TaskTracker.Application.DTOs.Validators;
using TaskTracker.Application.DTOs;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace TaskTracker.API.Extensions
{
	public static class FluentValidationConfig
	{
		public static void RegisterFluentValidationConfig(this IServiceCollection services)
		{
			services.AddScoped<IValidator<ProjectCreateRequest>, ProjectCreateRequestValidator>();
			services.AddScoped<IValidator<ProjectUpdateRequest>, ProjectUpdateRequestValidator>();
			services.AddScoped<IValidator<TaskCreateRequest>, TaskCreateRequestValidator>();
			services.AddScoped<IValidator<TaskUpdateRequest>, TaskUpdateRequestValidator>();
			
			services.AddFluentValidationAutoValidation();
		}
	}
}
