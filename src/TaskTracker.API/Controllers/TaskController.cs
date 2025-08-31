using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.API.Controllers
{
	[ApiController]
	[Route("api/tasks")]
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService;
		private readonly ILogger<TaskController> _logger;

		public TaskController(
			ITaskService taskService, 
			ILogger<TaskController> logger)
		{
			_taskService = taskService;
			_logger = logger;
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(TaskCreateRequest taskCreate)
		{
			_logger.LogInformation("Обращение к методу Create.");

			return await _taskService.Create(taskCreate);
		}

		[HttpGet("{taskId:guid}")]
		public async Task<ActionResult<TaskResponse>> GetById([FromRoute] Guid taskId)
		{
			_logger.LogInformation("Обращение к методу GetById.");

			return await _taskService.GetById(taskId);
		}

		[HttpGet]
		public async Task<ActionResult<List<TaskResponse>>> GetAll(
			[FromQuery] bool? isCompleted = null, 
			[FromQuery] Guid? projectId = null)
		{
			_logger.LogInformation("Обращение к методу GetAll.");

			return await _taskService.GetAll(isCompleted, projectId);
		}

		[HttpDelete]
		public async Task<ActionResult<int>> Delete(Guid taskId)
		{
			_logger.LogInformation("Обращение к методу Delete.");

			return await _taskService.Delete(taskId);
		}

		[HttpPut("{taskId:guid}")]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid taskId, 
			[FromBody] TaskUpdateRequest taskUpdate)
		{
			_logger.LogInformation("Обращение к методу Update.");

			return await _taskService.Update(taskId, taskUpdate);
		}
	}
}
