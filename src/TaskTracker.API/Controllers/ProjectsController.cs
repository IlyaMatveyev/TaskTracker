using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;

		public ProjectsController(IProjectService projectService)
		{
			_projectService = projectService;
		}

		[HttpGet]
		public async Task<ActionResult<List<ProjectResponse>>> GetAll()
		{
			var projectResponseList = await _projectService.GetAll();

			return Ok(projectResponseList);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(ProjectCreateRequest projectCreate)
		{
			return await _projectService.Create(projectCreate);
		}
	}
}
