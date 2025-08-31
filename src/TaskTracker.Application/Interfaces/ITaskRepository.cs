using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Interfaces
{
	public interface ITaskRepository
	{
		Task<Guid> Create(Task task);
		Task<Task> GetById(Guid taskId);
		Task<List<Task>> GetAll(bool? isCompleted, Guid? projectId);
		Task<int> Delete(Guid taskId);
		Task<Guid> Update(Guid taskId, Task taskUpdate);
	}
}
