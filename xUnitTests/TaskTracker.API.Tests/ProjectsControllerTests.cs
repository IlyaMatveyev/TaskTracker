using TaskTracker.API.Controllers;
using TaskTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Models;

namespace TaskTracker.API.Tests
{
	public class ProjectsControllerTests
	{
		private readonly Mock<IProjectService> _mockProjectService;
		private readonly Mock<ILogger<ProjectsController>> _mockLogger;
		private readonly Mock<ICacheService> _mockCacheService;
		private readonly ProjectsController _controller;

		public ProjectsControllerTests()
		{
			_mockProjectService = new Mock<IProjectService>();
			_mockLogger = new Mock<ILogger<ProjectsController>>();
			_mockCacheService = new Mock<ICacheService>();
			_controller = new ProjectsController(
				_mockProjectService.Object,
				_mockLogger.Object,
				_mockCacheService.Object);
		}

		#region GetAllTest

		[Fact]
		public async SysTask GetAll_WithValidPagination_ReturnsOkWithProjects()
		{
			// Arrange.
			var page = 1;
			var pageSize = 10;
			var projects = new List<ProjectResponse>
			{
				new ProjectResponse(
					Id: Guid.NewGuid(),
					Name: "Project 1",
					Description: "Description 1",
					CreatedAt: DateTime.UtcNow),
				new ProjectResponse(
					Id: Guid.NewGuid(),
					Name: "Project 2",
					Description: "Description 2",
					CreatedAt: DateTime.UtcNow)
			};

			var pagedResponse = new PagedResponse<ProjectResponse>
			{
				Items = projects,
				Page = page,
				PageSize = pageSize,
				TotalCount = 2
			};

			_mockCacheService.Setup(cache => cache.GetCachedValue(
					It.IsAny<string>(),
					It.IsAny<Func<Task<PagedResponse<ProjectResponse>>>>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
				.ReturnsAsync(pagedResponse);

			// Act.
			var result = await _controller.GetAll(page, pageSize);

			// Assert.
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result  as OkObjectResult; // ѕриводим к OkObjectResult чтобы получить данные из ответа.
			okResult.Value.Should().BeEquivalentTo(pagedResponse);

			_mockCacheService.Verify(cache => cache.GetCachedValue(
				$"projects_page{page}_pageSize{pageSize}",
				It.IsAny<Func<Task<PagedResponse<ProjectResponse>>>>(),
				TimeSpan.FromSeconds(60),
				TimeSpan.FromMinutes(5)), 
				Times.Once);
		}

		[Fact]
		public async SysTask GetAll_WithEmptyProjectsList_ReturnsNoContent()
		{
			// Arrange.
			var page = 1;
			var pageSize = 10;

			var emptyPagedResponse = new PagedResponse<ProjectResponse>
			{
				Items = new List<ProjectResponse>(),
				Page = page,
				PageSize = pageSize,
				TotalCount = 0
			};

			_mockCacheService.Setup(cache => cache.GetCachedValue(
					It.IsAny<string>(),
					It.IsAny<Func<Task<PagedResponse<ProjectResponse>>>>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
				.ReturnsAsync(emptyPagedResponse);

			// Act.
			var result = await _controller.GetAll(page, pageSize);

			// Assert.
			result.Result.Should().BeOfType<NoContentResult>();
		}

		[Theory]
		[InlineData(0, 10)]
		[InlineData(1, 0)]
		[InlineData(-1, 10)]
		[InlineData(1, -5)]
		public async SysTask GetAll_WithInvalidPaginationParams_ReturnsBadRequest(int invalidPage, int invalidPageSize)
		{
			// Act
			var result = await _controller.GetAll(invalidPage, invalidPageSize);

			// Assert
			result.Result.Should().BeOfType<BadRequestObjectResult>();

			_mockCacheService.Verify(cache => cache.GetCachedValue(
				It.IsAny<string>(),
				It.IsAny<Func<Task<PagedResponse<ProjectResponse>>>>(),
				It.IsAny<TimeSpan>(),
				It.IsAny<TimeSpan>()), 
				Times.Never);
		}

		#endregion

		#region GetByIdTest

		[Fact]
		public async SysTask GetById_WithExistingProjectId_ReturnsOkWithProject()
		{
			// Arrange
			var projectId = Guid.NewGuid();
			var taskResponse = new TaskResponse(
				Id: Guid.NewGuid(),
				Title: "Task 1",
				Description: "Description 1",
				IsCompleted: false,
				CreatedAt: DateTime.UtcNow,
				UpdatedAt: DateTime.UtcNow,
				ProjectId: projectId);

			var projectResponse = new ProjectWithTasksResponse(
				Id: projectId,
				Name: "Test Project",
				Description: "Test Description",
				CreatedAt: DateTime.UtcNow,
				Tasks: new List<TaskResponse> { taskResponse });

			_mockProjectService.Setup(service => service.GetById(projectId))
				.ReturnsAsync(projectResponse);

			// Act
			var result = await _controller.GetById(projectId);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().BeEquivalentTo(projectResponse);

			_mockProjectService.Verify(service => service.GetById(projectId), Times.Once);
		}

		[Fact]
		public async SysTask GetById_WithNonExistingProjectId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var nonExistingProjectId = Guid.NewGuid();

			_mockProjectService.Setup(service => service.GetById(nonExistingProjectId))
				.ThrowsAsync(new KeyNotFoundException($"Project was not found."));

			// Act & Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.GetById(nonExistingProjectId));

			_mockProjectService.Verify(service => service.GetById(nonExistingProjectId), Times.Once);
		}

		[Fact]
		public async SysTask GetById_WithEmptyGuid_ThrowsKeyNotFoundException()
		{
			// Arrange
			var emptyGuid = Guid.Empty;

			_mockProjectService.Setup(service => service.GetById(emptyGuid))
				.ThrowsAsync(new KeyNotFoundException("Project was not found"));

			// Act & Assert
			await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.GetById(emptyGuid));

			_mockProjectService.Verify(service => service.GetById(emptyGuid), Times.Once);
		}

		[Fact]
		public async SysTask GetById_WithProjectWithoutTasks_ReturnsOkWithEmptyTasksList()
		{
			// Arrange
			var projectId = Guid.NewGuid();
			var projectResponse = new ProjectWithTasksResponse(
				Id: projectId,
				Name: "Project",
				Description: "Project without tasks",
				CreatedAt: DateTime.UtcNow,
				Tasks: new List<TaskResponse>());

			_mockProjectService.Setup(service => service.GetById(projectId))
				.ReturnsAsync(projectResponse);

			// Act
			var result = await _controller.GetById(projectId);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			var returnedProject = okResult.Value as ProjectWithTasksResponse;

			returnedProject.Should().NotBeNull();
			returnedProject.Tasks.Should().BeEmpty();
			returnedProject.Id.Should().Be(projectId);

			_mockProjectService.Verify(service => service.GetById(projectId), Times.Once);
		}

		#endregion

		#region CreateTest

		[Fact]
		public async SysTask Create_WithValidRequest_ReturnsCreatedWithId()
		{
			// Arrange
			var projectCreate = new ProjectCreateRequest("New Project", "Description");
			var newProjectId = Guid.NewGuid();

			_mockProjectService.Setup(service => service.Create(projectCreate))
				.ReturnsAsync(newProjectId);

			// Act
			var result = await _controller.Create(projectCreate);

			// Assert
			result.Result.Should().BeOfType<CreatedAtActionResult>();
			var createdAtResult = result.Result as CreatedAtActionResult;

			createdAtResult.ActionName.Should().Be(nameof(ProjectsController.GetById));
			createdAtResult.RouteValues["projectId"].Should().Be(newProjectId);
			createdAtResult.Value.Should().Be(newProjectId);

			_mockProjectService.Verify(service => service.Create(projectCreate), Times.Once);
		}

		[Fact]
		public async SysTask Create_WithNameAndDescriptionMaxLength_ReturnsCreated()
		{
			// Arrange
			var maxLengthName = new string('A', Project.MAX_NAME_LENGTH);
			var maxLengthDescription = new string('A', Project.MAX_DESCRIPTION_LENGTH);
			var projectCreate = new ProjectCreateRequest(maxLengthName, maxLengthDescription);
			var newProjectId = Guid.NewGuid();

			_mockProjectService.Setup(service => service.Create(projectCreate))
				.ReturnsAsync(newProjectId);

			// Act
			var result = await _controller.Create(projectCreate);

			// Assert
			result.Result.Should().BeOfType<CreatedAtActionResult>();
			var createdAtResult = result.Result as CreatedAtActionResult;
			createdAtResult.Value.Should().Be(newProjectId);

			_mockProjectService.Verify(service => service.Create(projectCreate), Times.Once);
		}

		#endregion

		#region UpdateTest

		[Fact]
		public async SysTask Update_WithValidRequest_ReturnsOkWithId()
		{
			// Arrange
			var projectId = Guid.NewGuid();
			var projectUpdate = new ProjectUpdateRequest("Updated Name", "Updated Description");
			var updatedProjectId = Guid.NewGuid();

			_mockProjectService.Setup(service => service.Update(projectId, projectUpdate))
				.ReturnsAsync(updatedProjectId);

			// Act
			var result = await _controller.Update(projectId, projectUpdate);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().Be(updatedProjectId);

			_mockProjectService.Verify(service => service.Update(projectId, projectUpdate), Times.Once);
		}

		[Fact]
		public async SysTask Update_WithNonExistingProjectId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var nonExistingProjectId = Guid.NewGuid();
			var projectUpdate = new ProjectUpdateRequest("Valid Name", "Valid Description");

			_mockProjectService.Setup(service => service.Update(nonExistingProjectId, projectUpdate))
				.ThrowsAsync(new KeyNotFoundException($"Project was not found"));

			// Act & Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.Update(nonExistingProjectId, projectUpdate));

			_mockProjectService.Verify(service => service.Update(nonExistingProjectId, projectUpdate), Times.Once);
		}

		#endregion

		#region DeleteTest

		[Fact]
		public async SysTask Delete_WithExistingProjectId_ReturnsNoContent()
		{
			// Arrange
			var projectId = Guid.NewGuid();

			_mockProjectService.Setup(service => service.Delete(projectId))
				.ReturnsAsync(1);

			// Act
			var result = await _controller.Delete(projectId);

			// Assert
			result.Should().BeOfType<NoContentResult>();
			_mockProjectService.Verify(service => service.Delete(projectId), Times.Once);
		}

		[Fact]
		public async SysTask Delete_WithNonExistingProjectId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var nonExistingProjectId = Guid.NewGuid();

			_mockProjectService.Setup(service => service.Delete(nonExistingProjectId))
				.ThrowsAsync(new KeyNotFoundException($"Project not found"));

			// Act & Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.Delete(nonExistingProjectId));

			
			_mockProjectService.Verify(service => service.Delete(nonExistingProjectId), Times.Once);
		}

		#endregion

	}
}