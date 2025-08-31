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
		private readonly ILogger<ProjectsController> _logger;

		public ProjectsController(
			IProjectService projectService, 
			IMapper mapper, 
			ILogger<ProjectsController> logger)
		{
			_projectService = projectService;
			_mapper = mapper;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<PagedResponse<ProjectResponse>>> GetAll(
			[FromQuery] int page = 1, 
			[FromQuery] int pageSize = 10)
		{
			_logger.LogInformation("Обращение к методу GetAll.");

			var paginationParams = new PaginationParams(page, pageSize);

			var pagedProjectResponse = await _projectService.GetAll(paginationParams);

			return Ok(pagedProjectResponse);
		}

		[HttpGet("{projectId:guid}")]
		public async Task<ActionResult<ProjectWithTasksResponse>> GetById(Guid projectId)
		{
			_logger.LogInformation("Обращение к методу GetById.");

			return await _projectService.GetById(projectId);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(ProjectCreateRequest projectCreate)
		{
			_logger.LogInformation("Обращение к методу Create.");

			return await _projectService.Create(projectCreate);
		}

		[HttpDelete("{projectId:guid}")]
		public async Task<ActionResult<int>> Delete([FromRoute] Guid projectId)
		{
			_logger.LogInformation("Обращение к методу Delete.");

			return await _projectService.Delete(projectId);
		}

		[HttpPut("{projectId:guid}")]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid projectId, 
			[FromBody] ProjectUpdateRequest projectUpdate)
		{
			_logger.LogInformation("Обращение к методу Update.");

			return await _projectService.Update(projectId, projectUpdate);
		}
	}
}
