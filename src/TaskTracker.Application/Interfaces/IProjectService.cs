using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Interfaces
{
	public interface IProjectService
	{
		Task<List<ProjectResponse>> GetAll();
		Task<Guid> Create(ProjectCreateRequest projectRequest);
		Task<int> Delete(Guid projectId);
	}
}
