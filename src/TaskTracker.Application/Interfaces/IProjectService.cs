using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Interfaces
{
	public interface IProjectService
	{
		Task<List<ProjectResponse>> GetAll();
		Task<Project> GetById(Guid projectId);
		Task<Guid> Create(ProjectCreateRequest projectRequest);
		Task<Guid> Update(Guid projectId, ProjectUpdateRequest projectUpdate);
		Task<int> Delete(Guid projectId);
	}
}
