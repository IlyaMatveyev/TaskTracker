using Mapster;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Mapping
{
	public class ProjectDtoModelMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// Project -> ProjectWithTasksResponse
			TypeAdapterConfig<Project, ProjectWithTasksResponse>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.Map(dest => dest.Tasks, src => src.Tasks)
				.MaxDepth(2);

			// Project -> ProjectResponse
			TypeAdapterConfig<Project, ProjectResponse>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.MaxDepth(2);


			// ProjectCreateRequest -> Project
			TypeAdapterConfig<ProjectCreateRequest, Project>
				.NewConfig()
				.Map(dest => dest.Id, _ => Guid.NewGuid())
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.CreatedAt, _ => DateTime.UtcNow);


			// ProjectUpdateRequest -> Project
			TypeAdapterConfig<ProjectUpdateRequest, Project>
				.NewConfig()
				.Map(dest => dest.Id, _ => Guid.Empty)
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.Description, src => src.Description);
		}
	}
}
