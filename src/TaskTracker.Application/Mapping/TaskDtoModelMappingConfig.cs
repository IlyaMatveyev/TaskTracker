using Mapster;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Mapping
{
	public class TaskDtoModelMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// TaskCreateRequest -> Task
			TypeAdapterConfig<TaskCreateRequest, Task>
				.NewConfig()
				.Map(dest => dest.Id, _ => Guid.NewGuid())
				.Map(dest => dest.Title, src => src.Title)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.IsCompleted, src => src.IsCompleted)
				.Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
				.Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow)
				.Map(dest => dest.ProjectId, src => src.ProjectId)
				.MaxDepth(2);

			// TaskUpdateRequest -> Task
			TypeAdapterConfig<TaskUpdateRequest, Task>
				.NewConfig()
				.Map(dest => dest.Title, src => src.Title)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.IsCompleted, src => src.IsCompleted)
				.Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow)
				.Map(dest => dest.ProjectId, src => src.ProjectId)
				.MaxDepth(2);

			// Task -> TaskResponse
			TypeAdapterConfig<Task, TaskResponse>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Title, src => src.Title)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.IsCompleted, src => src.IsCompleted)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
				.Map(dest => dest.ProjectId, src => src.ProjectId)
				.MaxDepth(2);
		}
	}
}
