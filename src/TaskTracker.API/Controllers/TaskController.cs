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
		public TaskController(ITaskService taskService)
		{
			_taskService = taskService;
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(TaskCreateRequest taskCreate)
		{
			return await _taskService.Create(taskCreate);
		}

		[HttpGet("{taskId:guid}")]
		public async Task<ActionResult<TaskResponse>> GetById([FromRoute] Guid taskId)
		{
			return await _taskService.GetById(taskId);
		}

		[HttpGet]
		public async Task<ActionResult<List<TaskResponse>>> GetAll(
			[FromQuery] bool? isCompleted = null, 
			[FromQuery] Guid? projectId = null)
		{
			return await _taskService.GetAll(isCompleted, projectId);
		}

		[HttpDelete]
		public async Task<ActionResult<int>> Delete(Guid taskId)
		{
			return await _taskService.Delete(taskId);
		}

		[HttpPut("{taskId:guid}")]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid taskId, 
			[FromBody] TaskUpdateRequest taskUpdate)
		{
			return await _taskService.Update(taskId, taskUpdate);
		}
	}
}
