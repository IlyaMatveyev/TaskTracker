using Mapster;
using MapsterMapper;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Services
{
	public class ProjectService : IProjectService
	{
		private readonly IProjectRepository _projectRepository;
		private readonly IMapper _mapper;
		public ProjectService(
			IProjectRepository projectRepository,
			IMapper mapper)
		{
			_projectRepository = projectRepository;
			_mapper = mapper;
		}

		public Task<Guid> Create(ProjectCreateRequest projectRequest)
		{
			var project = _mapper.Map<Project>(projectRequest);

			return _projectRepository.Create(project);
		}

		public async Task<PagedResponse<ProjectResponse>> GetAll(PaginationParams paginationParams)
		{
			var pagedProject = await _projectRepository.GetAll(paginationParams);

			var pagedProjectResponse = new PagedResponse<ProjectResponse>()
			{
				Items = pagedProject.Items.Adapt<List<ProjectResponse>>(),
				Page = pagedProject.Page,
				PageSize = pagedProject.PageSize,
				TotalCount = pagedProject.TotalCount
			};

			return pagedProjectResponse;
		}

		public async Task<ProjectWithTasksResponse> GetById(Guid projectId)
		{
			var project = await _projectRepository.GetById(projectId);

			var projectWithTasksResponse = _mapper.Map<ProjectWithTasksResponse>(project);

			return projectWithTasksResponse;
		}

		public async Task<int> Delete(Guid projectId)
		{
			return await _projectRepository.Delete(projectId);
		}

		public async Task<Guid> Update(Guid projectId, ProjectUpdateRequest projectUpdate)
		{
			var project = _mapper.Map<Project>(projectUpdate);

			return await _projectRepository.Update(projectId, project);
		}
	}
}
