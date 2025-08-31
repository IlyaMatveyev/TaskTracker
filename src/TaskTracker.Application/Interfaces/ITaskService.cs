using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Interfaces
{
	public interface ITaskService
	{
		Task<Guid> Create(TaskCreateRequest taskCreate);
		Task<TaskResponse> GetById(Guid taskId);
	}
}
