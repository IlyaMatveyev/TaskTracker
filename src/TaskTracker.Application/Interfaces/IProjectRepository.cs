using TaskTracker.Application.DTOs.Common;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Interfaces
{
	public interface IProjectRepository
	{
		Task<PagedResponse<Project>> GetAll(PaginationParams paginationParams);
		Task<Project> GetById(Guid projectId);
		Task<Guid> Create(Project project);
		Task<int> Delete(Guid id);
		Task<Guid> Update(Guid id, Project project);
	}
}
