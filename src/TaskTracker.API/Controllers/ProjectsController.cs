using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.API.Controllers
{
	[ApiController]
	[Route("api/projects")]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;

		public ProjectsController(IProjectService projectService)
		{
			_projectService = projectService;
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

		[HttpGet("/{projectId:guid}")]
		public async Task<ActionResult<ProjectWithTasksResponse>> GetById(Guid projectId)
		{
			var project = await _projectService.GetById(projectId);

			// mapping project -> ProjectWithTasksResponse
			var projectWithTasksResponse = new ProjectWithTasksResponse
			(
				project.Id,
				project.Name,
				project.Description,
				project.CreatedAt,
				project.Tasks
			);

			return Ok(projectWithTasksResponse);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(ProjectCreateRequest projectCreate)
		{
			return await _projectService.Create(projectCreate);
		}

		[HttpDelete("/{projectId:guid}")]
		public async Task<ActionResult<int>> Delete([FromRoute] Guid projectId)
		{
			return await _projectService.Delete(projectId);
		}

		[HttpPut("/{projectId:guid}")]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid projectId, 
			[FromBody] ProjectUpdateRequest projectUpdate)
		{
			return await _projectService.Update(projectId, projectUpdate);
		}
	}
}
