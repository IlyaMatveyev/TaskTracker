using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskTracker.API.Controllers;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Models;

namespace TaskTracker.API.Tests
{
	public class TaskControllerTests
	{
		private readonly Mock<ITaskService> _mockTaskService;
		private readonly Mock<ILogger<TaskController>> _mockLogger;
		private readonly Mock<ICacheService> _mockCacheService;
		private readonly TaskController _controller;

		public TaskControllerTests()
		{
			_mockTaskService = new Mock<ITaskService>();
			_mockLogger = new Mock<ILogger<TaskController>>();
			_mockCacheService = new Mock<ICacheService>();
			_controller = new TaskController(
				_mockTaskService.Object,
				_mockLogger.Object,
				_mockCacheService.Object);
		}

		#region GetAllTest

		[Fact]
		public async SysTask GetAll_WithNoFilters_ReturnsOkWithTasks()
		{
			// Arrange
			bool? isCompleted = null;
			Guid? projectId = null;

			var tasks = new List<TaskResponse>
			{
				new TaskResponse(Guid.NewGuid(), "Task 1", "Description 1", false, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid()),
				new TaskResponse(Guid.NewGuid(), "Task 2", "Description 2", true, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid())
			};

			_mockCacheService.Setup(cache => cache.GetCachedValue(
					It.IsAny<string>(),
					It.IsAny<Func<Task<List<TaskResponse>>>>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
				.ReturnsAsync(tasks);

			// Act
			var result = await _controller.GetAll(isCompleted, projectId);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().BeEquivalentTo(tasks);

			_mockCacheService.Verify(cache => cache.GetCachedValue(
				$"tasks_{isCompleted}_{projectId}",
				It.IsAny<Func<Task<List<TaskResponse>>>>(),
				TimeSpan.FromSeconds(60),
				TimeSpan.FromMinutes(5)), 
				Times.Once);
		}

		[Fact]
		public async SysTask GetAll_WithIsCompletedFilter_ReturnsOkWithFilteredTasks()
		{
			// Arrange
			bool isCompleted = true;
			Guid? projectId = null;

			var tasks = new List<TaskResponse>
			{
				new TaskResponse(Guid.NewGuid(), "Completed Task1", "Description", true, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid()),
				new TaskResponse(Guid.NewGuid(), "Completed Task2", "Description", true, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid()),
				new TaskResponse(Guid.NewGuid(), "Completed Task3", "Description", true, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid()),
			};

			_mockCacheService.Setup(cache => cache.GetCachedValue(
					It.IsAny<string>(),
					It.IsAny<Func<Task<List<TaskResponse>>>>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
				.ReturnsAsync(tasks);

			// Act
			var result = await _controller.GetAll(isCompleted, projectId);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().BeEquivalentTo(tasks);

			_mockCacheService.Verify(cache => cache.GetCachedValue(
				$"tasks_{isCompleted}_{projectId}",
				It.IsAny<Func<Task<List<TaskResponse>>>>(),
				TimeSpan.FromSeconds(60),
				TimeSpan.FromMinutes(5)), 
				Times.Once);
		}

		[Fact]
		public async SysTask GetAll_WithProjectIdFilter_ReturnsOkWithFilteredTasks()
		{
			// Arrange
			bool? isCompleted = null;
			var projectId = Guid.NewGuid();

			var tasks = new List<TaskResponse>
			{
				new TaskResponse(Guid.NewGuid(), "Task 1", "Description 1", false, DateTime.UtcNow, DateTime.UtcNow, projectId),
				new TaskResponse(Guid.NewGuid(), "Task 2", "Description 2", true, DateTime.UtcNow, DateTime.UtcNow, projectId)
			};

			_mockCacheService.Setup(cache => cache.GetCachedValue(
					It.IsAny<string>(),
					It.IsAny<Func<Task<List<TaskResponse>>>>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
				.ReturnsAsync(tasks);

			// Act
			var result = await _controller.GetAll(isCompleted, projectId);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().BeEquivalentTo(tasks);

			_mockCacheService.Verify(cache => cache.GetCachedValue(
				$"tasks_{isCompleted}_{projectId}",
				It.IsAny<Func<Task<List<TaskResponse>>>>(),
				TimeSpan.FromSeconds(60),
				TimeSpan.FromMinutes(5)), 
				Times.Once);
		}

		[Fact]
		public async SysTask GetAll_WithBothFilters_ReturnsOkWithFilteredTasks()
		{
			// Arrange
			bool isCompleted = false;
			var projectId = Guid.NewGuid();

			var tasks = new List<TaskResponse>
			{
				new TaskResponse(Guid.NewGuid(), "Task1", "Description", false, DateTime.UtcNow, DateTime.UtcNow, projectId),
				new TaskResponse(Guid.NewGuid(), "Task2", "Description", false, DateTime.UtcNow, DateTime.UtcNow, projectId),
				new TaskResponse(Guid.NewGuid(), "Task3", "Description", false, DateTime.UtcNow, DateTime.UtcNow, projectId)
			};

			_mockCacheService.Setup(cache => cache.GetCachedValue(
					It.IsAny<string>(),
					It.IsAny<Func<Task<List<TaskResponse>>>>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
				.ReturnsAsync(tasks);

			// Act
			var result = await _controller.GetAll(isCompleted, projectId);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().BeEquivalentTo(tasks);

			_mockCacheService.Verify(cache => cache.GetCachedValue(
				$"tasks_{isCompleted}_{projectId}",
				It.IsAny<Func<Task<List<TaskResponse>>>>(),
				TimeSpan.FromSeconds(60),
				TimeSpan.FromMinutes(5)), Times.Once);
		}

		#endregion

		#region GetByIdTest

		[Fact]
		public async SysTask GetById_WithExistingTaskId_ReturnsOkWithTask()
		{
			// Arrange
			var taskResponse = new TaskResponse(
				Id: Guid.NewGuid(),
				Title: "Task 1",
				Description: "Description 1",
				IsCompleted: false,
				CreatedAt: DateTime.UtcNow,
				UpdatedAt: DateTime.UtcNow,
				ProjectId: Guid.NewGuid());


			_mockTaskService.Setup(service => service.GetById(taskResponse.Id))
				.ReturnsAsync(taskResponse);

			// Act
			var result = await _controller.GetById(taskResponse.Id);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().BeEquivalentTo(taskResponse);

			_mockTaskService.Verify(service => service.GetById(taskResponse.Id), Times.Once);
		}

		[Fact]
		public async SysTask GetById_WithNonExistingTaskId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var nonExistingTaskId = Guid.NewGuid();

			_mockTaskService.Setup(service => service.GetById(nonExistingTaskId))
				.ThrowsAsync(new KeyNotFoundException($"Task was not found."));

			// Act & Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.GetById(nonExistingTaskId));

			_mockTaskService.Verify(service => service.GetById(nonExistingTaskId), Times.Once);
		}

		[Fact]
		public async SysTask GetById_WithEmptyTaskId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var emptyGuid = Guid.Empty;

			_mockTaskService.Setup(service => service.GetById(emptyGuid))
				.ThrowsAsync(new KeyNotFoundException("Task was not found"));

			// Act & Assert
			await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.GetById(emptyGuid));

			_mockTaskService.Verify(service => service.GetById(emptyGuid), Times.Once);
		}

		#endregion

		#region CreateTest

		[Fact]
		public async SysTask Create_WithValidRequest_ReturnsCreatedWithId()
		{
			// Arrange
			var taskCreate = new TaskCreateRequest("New task", "Description", false, Guid.NewGuid());
			var newTaskId = Guid.NewGuid();

			_mockTaskService.Setup(service => service.Create(taskCreate))
				.ReturnsAsync(newTaskId);

			// Act
			var result = await _controller.Create(taskCreate);

			// Assert
			result.Result.Should().BeOfType<CreatedAtActionResult>();
			var createdAtResult = result.Result as CreatedAtActionResult;

			createdAtResult.ActionName.Should().Be(nameof(TaskController.GetById));
			createdAtResult.RouteValues["taskId"].Should().Be(newTaskId);
			createdAtResult.Value.Should().Be(newTaskId);

			_mockTaskService.Verify(service => service.Create(taskCreate), Times.Once);
		}

		[Fact]
		public async SysTask Create_WithTitleAndDescriptionMaxLength_ReturnsCreated()
		{
			// Arrange
			var maxLengthName = new string('A', Task.MAX_TITLE_LENGTH);
			var maxLengthDescription = new string('A', Task.MAX_DESCRIPTION_LENGTH);
			var taskCreate = new TaskCreateRequest(maxLengthName, maxLengthDescription, false, Guid.NewGuid());
			var newTaskId = Guid.NewGuid();

			_mockTaskService.Setup(service => service.Create(taskCreate))
				.ReturnsAsync(newTaskId);

			// Act
			var result = await _controller.Create(taskCreate);

			// Assert
			result.Result.Should().BeOfType<CreatedAtActionResult>();
			var createdAtResult = result.Result as CreatedAtActionResult;


			createdAtResult.ActionName.Should().Be(nameof(TaskController.GetById));
			createdAtResult.RouteValues["taskId"].Should().Be(newTaskId);
			createdAtResult.Value.Should().Be(newTaskId);

			_mockTaskService.Verify(service => service.Create(taskCreate), Times.Once);
		}

		#endregion

		#region UpdateTest

		[Fact]
		public async SysTask Update_WithValidRequest_ReturnsOkWithId()
		{
			// Arrange
			var taskId = Guid.NewGuid();
			var taskUpdate = new TaskUpdateRequest("New task name", "new Description", true, Guid.NewGuid());

			_mockTaskService.Setup(service => service.Update(taskId, taskUpdate))
				.ReturnsAsync(taskId);

			// Act
			var result = await _controller.Update(taskId, taskUpdate);

			// Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			okResult.Value.Should().Be(taskId);

			_mockTaskService.Verify(service => service.Update(taskId, taskUpdate), Times.Once);
		}

		[Fact]
		public async SysTask Update_WithNonExistingProjectId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var nonExistingTaskId = Guid.NewGuid();
			var taskUpdate = new TaskUpdateRequest("New task name", "new Description", true, Guid.NewGuid());

			_mockTaskService.Setup(service => service.Update(nonExistingTaskId, taskUpdate))
				.ThrowsAsync(new KeyNotFoundException($"Task was not found"));

			// Act & Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.Update(nonExistingTaskId, taskUpdate));

			_mockTaskService.Verify(service => service.Update(nonExistingTaskId, taskUpdate), Times.Once);
		}

		#endregion

		#region DeleteTest

		[Fact]
		public async SysTask Delete_WithExistingTaskId_ReturnsNoContent()
		{
			// Arrange
			var taskId = Guid.NewGuid();

			_mockTaskService.Setup(service => service.Delete(taskId))
				.ReturnsAsync(1);

			// Act
			var result = await _controller.Delete(taskId);

			// Assert
			result.Should().BeOfType<NoContentResult>();
			_mockTaskService.Verify(service => service.Delete(taskId), Times.Once);
		}

		[Fact]
		public async SysTask Delete_WithNonExistingTaskId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var nonExistingTaskId = Guid.NewGuid();

			_mockTaskService.Setup(service => service.Delete(nonExistingTaskId))
				.ThrowsAsync(new KeyNotFoundException($"Task not found"));

			// Act & Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
				_controller.Delete(nonExistingTaskId));


			_mockTaskService.Verify(service => service.Delete(nonExistingTaskId), Times.Once);
		}

		#endregion

	}
}
