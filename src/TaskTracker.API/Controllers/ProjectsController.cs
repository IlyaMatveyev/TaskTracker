using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.API.Controllers
{
	/// <summary>
	/// Контроллер для работы с объектами типа Project.
	/// </summary>
	[ApiController]
	[Route("api/projects")]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;
		private readonly ILogger<ProjectsController> _logger;

		public ProjectsController(
			IProjectService projectService, 
			IMapper mapper, 
			ILogger<ProjectsController> logger)
		{
			_projectService = projectService;
			_logger = logger;
		}

		/// <summary>
		/// Получить список проектов (с поддержкой пагинации).
		/// </summary>
		/// <param name="page">Номер страницы.</param>
		/// <param name="pageSize">Кол-во записей на одной странице.</param>
		/// <returns>Объект содержащий коллекцию Project и информацию о пагинации.</returns>
		/// <response code="200">Список проектов найден и возвращён.</response>
		/// <response code="204">Список проектов пуст, body - пусто.</response>
		/// <response code="400">Переданы некорректные данные.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpGet]
		[ProducesResponseType(typeof(PagedResponse<ProjectResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<PagedResponse<ProjectResponse>>> GetAll(
			[FromQuery] int page = 1, 
			[FromQuery] int pageSize = 10)
		{
			_logger.LogInformation("Обращение к методу GetAll.");

			var paginationParams = new PaginationParams(page, pageSize);

			var pagedProjectResponse = await _projectService.GetAll(paginationParams);

			if (pagedProjectResponse.Items.Count == 0)
			{
				return NoContent();
			}

			return Ok(pagedProjectResponse);
		}

		/// <summary>
		/// Получить проект по идентификатору.
		/// </summary>
		/// <param name="projectId">Идентификатор проекта.</param>
		/// <returns>Детали проекта.</returns>
		/// <response code="200">Проект найден и возвращен.</response>
		/// <response code="404">Проект с указанным идентификатором не найден.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpGet("{projectId:guid}")]
		[ProducesResponseType(typeof(ProjectWithTasksResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<ProjectWithTasksResponse>> GetById([FromRoute] Guid projectId)
		{
			_logger.LogInformation("Обращение к методу GetById.");

			return Ok(await _projectService.GetById(projectId));
		}

		/// <summary>
		/// Создать новый проект.
		/// </summary>
		/// <param name="projectCreate">Модель данных для создания проекта.</param>
		/// <returns>Идентификатор созданного проекта.</returns>
		/// <response code="201">Проект успешно создан.</response>
		/// <response code="400">Переданы некорректные данные.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpPost]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Guid>> Create([FromBody] ProjectCreateRequest projectCreate)
		{
			_logger.LogInformation("Обращение к методу Create.");

			var newId = await _projectService.Create(projectCreate);

			return CreatedAtAction(
				nameof(GetById), 
				new { projectId = newId }, 
				newId);
		}

		/// <summary>
		/// Удалить проект по идентификатору.
		/// </summary>
		/// <param name="projectId">Идентификатор проекта.</param>
		/// <response code="204">Проект удален.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpDelete("{projectId:guid}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete([FromRoute] Guid projectId)
		{
			_logger.LogInformation("Обращение к методу Delete.");

			await _projectService.Delete(projectId);

			return NoContent();
		}

		/// <summary>
		/// Обновить данные о проекте.
		/// </summary>
		/// <param name="projectId">Идентификатор проекта.</param>
		/// <param name="projectUpdate">Модель данных для обновления проекта.</param>
		/// <returns>Идентификатор обновлённого проекта.</returns>
		/// <response code="200">Проект успешно обновлен.</response>
		/// <response code="400">Переданы некорректные данные.</response>
		/// <response code="404">Проект для обновления не найден.</response>
		/// <response code="500">Внутренняя ошибка сервера.</response>
		[HttpPut("{projectId:guid}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Guid>> Update(
			[FromRoute] Guid projectId, 
			[FromBody] ProjectUpdateRequest projectUpdate)
		{
			_logger.LogInformation("Обращение к методу Update.");

			return Ok(await _projectService.Update(projectId, projectUpdate));
		}
	}
}
