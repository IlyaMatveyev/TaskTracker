using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Interfaces
{
	public interface IProjectRepository
	{
		Task<List<Project>> GetAll();
		Task<Project> GetById(Guid projectId);
		Task<Guid> Create(Project project);
		Task<int> Delete(Guid id);
		Task<Guid> Update(Guid id, Project project);
	}
}
