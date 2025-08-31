using System.Web.Http;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Interfaces
{
	public interface ITaskService
	{
		Task<Guid> Create(TaskCreateRequest taskCreate);
		Task<TaskResponse> GetById(Guid taskId);
		Task<List<TaskResponse>> GetAll(bool? isCompleted, Guid? projectId);
		Task<int> Delete(Guid taskId);
		Task<Guid> Update(Guid taskId, TaskUpdateRequest taskUpdate);
	}
}
