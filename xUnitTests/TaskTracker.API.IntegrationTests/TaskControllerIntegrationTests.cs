using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TaskTracker.Application.DTOs;
using TaskTracker.Infrastructure.PostgreSqlDb;
using System.Net;
using Microsoft.EntityFrameworkCore;
using TaskTracker.API.IntegrationTests.Common;

namespace TaskTracker.API.IntegrationTests
{
	public class TaskControllerIntegrationTests : IClassFixture<DatabaseFixture>
	{
		private TestWebApplicationFactory _applicationFactory;
		private HttpClient _client;

		public TaskControllerIntegrationTests(DatabaseFixture databaseFixture)
		{
			_applicationFactory = new TestWebApplicationFactory(databaseFixture.ConnectionString, databaseFixture.MapsterTestConfig);
			_client = _applicationFactory.CreateClient();
		}

		private async SysTask ClearDatabase()
		{
			await using var scope = _applicationFactory.Services.CreateAsyncScope();
			var db = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();
			await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Tasks\", \"Projects\" RESTART IDENTITY CASCADE;");
		}

		#region GetAllTests

		[Fact]
		public async SysTask GetAll_DbContainsTasks_ReturnsOkWithTasks()
		{
			// Arrange.
			await ClearDatabase();

			await using var scope = _applicationFactory.Services.CreateAsyncScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Добавление тестовых данных в бд.
			var projectId = Guid.NewGuid();
			dbContext.Projects.Add(new Infrastructure.Entities.ProjectEntity
			{
				Id = projectId,
				Name = "proj 1",
				Description = "desc 1",
				CreatedAt = DateTime.UtcNow
			});
			dbContext.Tasks.Add(new Infrastructure.Entities.TaskEntity
			{
				Id = Guid.NewGuid(),
				Title = "Task 1",
				Description = "task description",
				IsCompleted = false,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				ProjectEntityId = projectId
			});
			await dbContext.SaveChangesAsync();

			// Act.
			var response = await _client.GetAsync("/api/tasks");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

			var tasks = await response.Content.ReadFromJsonAsync<List<TaskResponse>>();
			tasks.Should().NotBeNull();
			tasks.Should().HaveCount(1);
			tasks[0].Title.Should().Be("Task 1");
			tasks[0].Description.Should().Be("task description");
		}

		[Fact]
		public async SysTask GetAll_DbNoContainsTasks_ReturnsNoContent()
		{
			// Arrange.
			await ClearDatabase();

			// Act.
			var response = await _client.GetAsync("/api/tasks?isCompleted=false");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}

		#endregion


		#region GetByIdTests

		[Fact]
		public async SysTask GetById_ExistingTaskId_ReturnsOkWithTask()
		{
			// Arrange.
			await ClearDatabase();

			await using var scope = _applicationFactory.Services.CreateAsyncScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Добавление тестовых данных в бд.
			var projectId = Guid.NewGuid();
			var taskId = Guid.NewGuid();

			dbContext.Projects.Add(new Infrastructure.Entities.ProjectEntity
			{
				Id = projectId,
				Name = "proj 1",
				Description = "desc 1",
				CreatedAt = DateTime.UtcNow
			});
			dbContext.Tasks.Add(new Infrastructure.Entities.TaskEntity
			{
				Id = taskId,
				Title = "Task 1",
				Description = "task description",
				IsCompleted = false,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				ProjectEntityId = projectId
			});
			await dbContext.SaveChangesAsync();

			// Act.
			var response = await _client.GetAsync($"/api/tasks/{taskId}");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

			var task = await response.Content.ReadFromJsonAsync<TaskResponse>();
			task.Should().NotBeNull();
			task.Title.Should().Be("Task 1");
		}

		[Fact]
		public async SysTask GetById_NoExistingTaskId_ReturnsNotFound()
		{
			// Arrange.
			await ClearDatabase();

			await using var scope = _applicationFactory.Services.CreateAsyncScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			var taskId = Guid.NewGuid();

			// Act.
			var response = await _client.GetAsync($"/api/tasks/{taskId}");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}

		#endregion


		#region CreateTests

		[Fact]
		public async SysTask Create_InvalidTitle_ReturnsBadRequest()
		{
			// Arrange.
			await ClearDatabase();

			await using var scope = _applicationFactory.Services.CreateAsyncScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			var projectId = Guid.NewGuid();
			var taskId = Guid.NewGuid();

			// Добавление тестовых данных в бд.
			dbContext.Projects.Add(new Infrastructure.Entities.ProjectEntity
			{
				Id = projectId,
				Name = "proj 1",
				Description = "desc 1",
				CreatedAt = DateTime.UtcNow
			});
			await dbContext.SaveChangesAsync();


			var taskCreate = new TaskCreateRequest
			(
				"",
				"description",
				false,
				projectId
			);

			// Act.
			var response = await _client.PostAsJsonAsync("/api/tasks", taskCreate);

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		#endregion

	}
}
