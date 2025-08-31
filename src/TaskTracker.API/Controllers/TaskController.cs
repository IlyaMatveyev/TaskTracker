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
		public async Task<Guid> Create(TaskCreateRequest taskCreate)
		{
			return await _taskService.Create(taskCreate);
		}
	}
}
