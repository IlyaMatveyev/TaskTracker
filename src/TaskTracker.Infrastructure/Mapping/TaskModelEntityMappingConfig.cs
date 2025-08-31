using Mapster;
using TaskTracker.Application.DTOs;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.Mapping
{
	public class TaskModelEntityMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// Task -> TaskEntity
			TypeAdapterConfig<Task, TaskEntity>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Title, src => src.Title)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.IsCompleted, src => src.IsCompleted)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
				.Map(dest => dest.ProjectEntityId, src => src.ProjectId)
				.Map(dest => dest.ProjectEntity, src => src.Project)
				.TwoWays()
				.MaxDepth(2);

			// TaskEntity -> Task
			TypeAdapterConfig<TaskEntity, Task>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Title, src => src.Title)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.IsCompleted, src => src.IsCompleted)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
				.Map(dest => dest.ProjectId, src => src.ProjectEntityId)
				.Map(dest => dest.Project, src => src.ProjectEntity)
				.TwoWays()
				.MaxDepth(2);
		}
	}
}
