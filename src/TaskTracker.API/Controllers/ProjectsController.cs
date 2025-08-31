using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.API.Controllers
{
	[ApiController]
	[Route("api/projects")]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;
		private readonly IMapper _mapper;

		public ProjectsController(
			IProjectService projectService, 
			IMapper mapper)
		{
			_projectService = projectService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<PagedResponse<ProjectResponse>>> GetAll(
			[FromQuery] int page = 1, 
			[FromQuery] int pageSize = 10)
		{
			var paginationParams = new PaginationParams(page, pageSize);

			var pagedProjectResponse = await _projectService.GetAll(paginationParams);

			return Ok(pagedProjectResponse);
		}

		[HttpGet("{projectId:guid}")]
		public async Task<ActionResult<ProjectWithTasksResponse>> GetById(Guid projectId)
		{
			return await _projectService.GetById(projectId);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(ProjectCreateRequest projectCreate)
		{
			return await _projectService.Create(projectCreate);
		}

		[HttpDelete("{projectId:guid}")]
		public async Task<ActionResult<int>> Delete([FromRoute] Guid projectId)
		{
			return await _projectService.Delete(projectId);
		}

		[HttpPut("{projectId:guid}")]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid projectId, 
			[FromBody] ProjectUpdateRequest projectUpdate)
		{
			return await _projectService.Update(projectId, projectUpdate);
		}
	}
}
