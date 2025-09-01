using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.API.Controllers
{
	/// <summary>
	/// Контроллер для работы с объектами типа Task.
	/// </summary>
	[ApiController]
	[Route("api/tasks")]
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService;
		private readonly ILogger<TaskController> _logger;
		private readonly ICacheService _cacheService;

		public TaskController(
			ITaskService taskService, 
			ILogger<TaskController> logger,
			ICacheService cacheService)
		{
			_taskService = taskService;
			_logger = logger;
			_cacheService = cacheService;
		}

		/// <summary>
		/// Создать новую задачу.
		/// </summary>
		/// <param name="taskCreate">Модель данных для создания задачи.</param>
		/// <returns>Идентификатор созданной задачи.</returns>
		/// <response code="201">Задача успешно создана.</response>
		/// <response code="400">Переданы некорректные данные.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpPost]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Guid>> Create([FromBody] TaskCreateRequest taskCreate)
		{
			_logger.LogInformation("Обращение к методу Create.");

			var newId = await _taskService.Create(taskCreate);

			return CreatedAtAction(
				nameof(GetById),
				new { taskId = newId },
				newId);
		}

		/// <summary>
		/// Получить задачу по идентификатору.
		/// </summary>
		/// <param name="taskId">Идентификатор задачи.</param>
		/// <returns>Детали задачи.</returns>
		/// <response code="200">Задача найдена и возвращена.</response>
		/// <response code="404">Задача с указанным идентификатором не найдена.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpGet("{taskId:guid}")]
		[ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TaskResponse>> GetById([FromRoute] Guid taskId)
		{
			_logger.LogInformation("Обращение к методу GetById.");

			return Ok(await _taskService.GetById(taskId));
		}

		/// <summary>
		/// Получить список задач.
		/// </summary>
		/// <param name="isCompleted">Флаг завершённости задачи.</param>
		/// <param name="projectId">Идентификатор проекта.</param>
		/// <returns>Список задач.</returns>
		/// <response code="200">Список задач найден и возвращён.</response>
		/// <response code="204">Список задач пуст, body - пусто.</response>
		/// <response code="400">Переданы некорректные данные.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpGet]
		[ProducesResponseType(typeof(List<TaskResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<TaskResponse>>> GetAll(
			[FromQuery] bool? isCompleted = null, 
			[FromQuery] Guid? projectId = null)
		{
			_logger.LogInformation("Обращение к методу GetAll.");

			var cacheKey = $"tasks_{isCompleted}_{projectId}";

			var taskResponseList = await _cacheService.GetCachedValue(
				cacheKey,
				async () => await _taskService.GetAll(isCompleted, projectId),
				TimeSpan.FromSeconds(60),
				TimeSpan.FromMinutes(5));

			if(taskResponseList.Count == 0)
			{
				return NoContent();
			}

			return Ok(taskResponseList);
		}

		/// <summary>
		/// Удалить задачу по идентификатору.
		/// </summary>
		/// <param name="taskId">Идентификатор задачи.</param>
		/// <response code="204">Задача удалена.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete([FromRoute] Guid taskId)
		{
			_logger.LogInformation("Обращение к методу Delete.");

			await _taskService.Delete(taskId);

			return NoContent();
		}

		/// <summary>
		/// Обновить задачу по идентификатору.
		/// </summary>
		/// <param name="taskId">Идентификатор задачи.</param>
		/// <param name="taskUpdate">Данные для обновления задачи.</param>
		/// <returns>Идентификатор обновлённой задачи.</returns>
		/// <response code="200">Задача успешно обновлена.</response>
		/// <response code="400">Переданы некорректные данные.</response>
		/// <response code="404">Задача для обновления не найдена.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpPut("{taskId:guid}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid taskId, 
			[FromBody] TaskUpdateRequest taskUpdate)
		{
			_logger.LogInformation("Обращение к методу Update.");

			return Ok(await _taskService.Update(taskId, taskUpdate));
		}
	}
}
