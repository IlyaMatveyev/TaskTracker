namespace TaskTracker.Application.Interfaces
{
	public interface ITaskRepository
	{
		Task<Guid> Create(Task task);
		Task<Task> GetById(Guid taskId);
	}
}
