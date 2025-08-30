using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Services
{
	public class ProjectService : IProjectService
	{
		private readonly IProjectRepository _projectRepository;
		public ProjectService(IProjectRepository projectRepository)
		{
			_projectRepository = projectRepository;
		}

		public Task<Guid> Create(ProjectCreateRequest projectRequest)
		{
			var project = new Project()
			{
				Id = Guid.NewGuid(),
				Name = projectRequest.Name,
				Description = projectRequest.Description,
				CreatedAt = DateTime.UtcNow
			};

			return _projectRepository.Create(project);
		}

		public async Task<List<ProjectResponse>> GetAll()
		{
			var projectList = await _projectRepository.GetAll();

			// маппинг
			var projectResponseList = projectList.Select(p => new ProjectResponse
			(
				p.Id,
				p.Name,
				p.Description,
				p.CreatedAt
			)).ToList();

			return projectResponseList;
		}

		public async Task<Project> GetById(Guid projectId)
		{
			var project = await _projectRepository.GetById(projectId);

			return project;
		}

		public async Task<int> Delete(Guid projectId)
		{
			return await _projectRepository.Delete(projectId);
		}

		public async Task<Guid> Update(Guid projectId, ProjectUpdateRequest projectUpdate)
		{
			var project = new Project()
			{
				Name = projectUpdate.Name,
				Description = projectUpdate.Description,
			};

			return await _projectRepository.Update(projectId, project);
		}
	}
}
