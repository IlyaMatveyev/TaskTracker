using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TaskTracker.API.IntegrationTests.Common;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Infrastructure.PostgreSqlDb;

namespace TaskTracker.API.IntegrationTests
{
	public class ProjectsControllerIntegrationTests : IClassFixture<DatabaseFixture>
	{
		private readonly TestWebApplicationFactory _applicationFactory;
		private readonly HttpClient _client;

		public ProjectsControllerIntegrationTests(DatabaseFixture databaseFixture)
		{
			//_fixture = databaseFixture;
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
		public async SysTask GetAll_DbContainsProjects_ReturnsOkWithProjects()
		{
			// Arrange.
			// Очитка бд от предыдущих тестов.
			await ClearDatabase();

			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Добавление тестовых данных в бд.
			context.Projects.Add(new Infrastructure.Entities.ProjectEntity
			{
				Id = Guid.NewGuid(),
				Name = "proj 1",
				Description = "desc 1",
				CreatedAt = DateTime.UtcNow
			});
			await context.SaveChangesAsync();

			// Act.
			var response = await _client.GetAsync("/api/projects");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

			var projects = await response.Content.ReadFromJsonAsync<PagedResponse<ProjectResponse>>();
			projects.Should().NotBeNull();
			projects.Items.Should().HaveCount(1);
			projects.Items[0].Name.Should().Be("proj 1");
		}

		[Fact]
		public async SysTask GetAll_DbNoContainsProjects_ReturnsNoContent()
		{
			// Arrange.
			// Очитка бд от предыдущих тестов.
			await ClearDatabase();

			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Act.
			var response = await _client.GetAsync("/api/projects?page=1&pageSize=5");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}

		[Fact]
		public async SysTask GetAll_InvalidPageValue_ReturnsBadRequest()
		{
			// Arrange.
			// Очитка бд от предыдущих тестов.
			await ClearDatabase();

			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Act.
			var response = await _client.GetAsync("/api/projects?page=-1&pageSize=5");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async SysTask GetAll_InvalidPageSizeValue_ReturnsBadRequest()
		{
			// Arrange.
			// Очитка бд от предыдущих тестов.
			await ClearDatabase();

			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Act.
			var response = await _client.GetAsync("/api/projects?page=1&pageSize=-5");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		#endregion

		#region GetByIdTests

		[Fact]
		public async SysTask GetById_ExistingProjectId_ReturnsOkWithProject()
		{
			// Arrange.
			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Очистка бд от предыдущих тестов.
			context.Projects.RemoveRange(context.Projects);
			await context.SaveChangesAsync();

			// Добавление тестовых данных в бд.
			var projectId = Guid.NewGuid();
			context.Projects.Add(new Infrastructure.Entities.ProjectEntity
			{
				Id = projectId,
				Name = "proj 1",
				Description = "desc 1",
				CreatedAt = DateTime.UtcNow
			});
			await context.SaveChangesAsync();

			// Act.
			var response = await _client.GetAsync($"/api/projects/{projectId}");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

			var projects = await response.Content.ReadFromJsonAsync<ProjectResponse>();
			projects.Should().NotBeNull();
			projects.Name.Should().Be("proj 1");
		}

		[Fact]
		public async SysTask GetById_NoExistingProjectId_ReturnsNotFound()
		{
			// Arrange.
			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Очитка бд от предыдущих тестов.
			context.Projects.RemoveRange(context.Projects);
			await context.SaveChangesAsync();

			var projectId = Guid.NewGuid();

			// Добавление тестовых данных в бд.
			context.Projects.Add(new Infrastructure.Entities.ProjectEntity
			{
				Id = Guid.NewGuid(),
				Name = "proj 1",
				Description = "desc 1",
				CreatedAt = DateTime.UtcNow
			});
			await context.SaveChangesAsync();

			// Act.
			var response = await _client.GetAsync($"/api/projects/{projectId}");

			// Assert.
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}

		#endregion

		#region CreateTests

		//[Fact]
		//public async SysTask Create_ValidData_ReturnsOkWithProjectId()
		//{
		//	// Arrange.
		//	using var scope = _applicationFactory.Services.CreateScope();
		//	var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

		//	// Очитка бд от предыдущих тестов.
		//	context.Projects.RemoveRange(context.Projects);
		//	await context.SaveChangesAsync();

		//	var projectCreate = new ProjectCreateRequest
		//	(
		//		"new project",
		//		"description"
		//	);

		//	// Act.
		//	var response = await _client.PostAsJsonAsync("/api/projects", projectCreate);

		//	// Assert.
		//	response.StatusCode.Should().Be(HttpStatusCode.Created);

			
		//	var createdId = await response.Content.ReadFromJsonAsync<Guid>();
		//	createdId.Should().NotBe(Guid.Empty);

		//	// Проверка объекта в бд.
		//	var entity = await context.Projects.FindAsync(createdId);
		//	entity.Should().NotBeNull();
		//	entity!.Name.Should().Be(projectCreate.Name);
		//	entity.Description.Should().Be(projectCreate.Description);
		//}

		[Fact]
		public async SysTask Create_InvalidName_ReturnsBadRequest()
		{
			// Arrange.
			using var scope = _applicationFactory.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			// Очитка бд от предыдущих тестов.
			context.Projects.RemoveRange(context.Projects);
			await context.SaveChangesAsync();

			var projectCreate = new ProjectCreateRequest
			(
				"",
				"description"
			);

			// Act.
			var response = await _client.PostAsJsonAsync("/api/projects", projectCreate);

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		#endregion

		#region UpdateTests

		[Fact]
		public async SysTask Update_ValidData_ReturnsOkWithProjectId()
		{
			// Arrange.
			var projectId = Guid.NewGuid();

			using(var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				// Очитка бд от предыдущих тестов.
				context.Projects.RemoveRange(context.Projects);
				await context.SaveChangesAsync();

				// Добавление тестовых данных в бд.
				context.Projects.Add(new Infrastructure.Entities.ProjectEntity
				{
					Id = projectId,
					Name = "proj 1",
					Description = "desc 1",
					CreatedAt = DateTime.UtcNow
				});
				await context.SaveChangesAsync();
			}

			var projectUpdate = new ProjectUpdateRequest
			(
				"new name for project",
				"new desription"
			);

			// Act.
			var response = await _client.PutAsJsonAsync($"/api/projects/{projectId}", projectUpdate);

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.OK);


			var updatedProjectId = await response.Content.ReadFromJsonAsync<Guid>();
			updatedProjectId.Should().Be(projectId);

			// Проверка объекта в бд.
			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				var entity = await context.Projects.FindAsync(projectId);
				entity.Should().NotBeNull();
				entity!.Name.Should().Be(projectUpdate.Name);
				entity.Description.Should().Be(projectUpdate.Description);
			}
		}

		[Fact]
		public async SysTask Update_InvalidData_ReturnsBadRequest()
		{
			// Arrange.
			var projectId = Guid.NewGuid();

			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				// Очитка бд от предыдущих тестов.
				context.Projects.RemoveRange(context.Projects);
				await context.SaveChangesAsync();

				// Добавление тестовых данных в бд.
				context.Projects.Add(new Infrastructure.Entities.ProjectEntity
				{
					Id = projectId,
					Name = "proj 1",
					Description = "desc 1",
					CreatedAt = DateTime.UtcNow
				});
				await context.SaveChangesAsync();
			}

			var projectUpdate = new ProjectUpdateRequest
			(
				"",
				"new desription"
			);

			// Act.
			var response = await _client.PutAsJsonAsync($"/api/projects/{projectId}", projectUpdate);

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async SysTask Update_NoExistingProjectId_ReturnsNotFound()
		{
			// Arrange.
			var projectId = Guid.NewGuid();

			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				// Очитка бд от предыдущих тестов.
				context.Projects.RemoveRange(context.Projects);
				await context.SaveChangesAsync();
			}

			var projectUpdate = new ProjectUpdateRequest
			(
				"valid name",
				"new desription"
			);

			// Act.
			var response = await _client.PutAsJsonAsync($"/api/projects/{projectId}", projectUpdate);

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		#endregion

		#region DeleteTests

		[Fact]
		public async SysTask Delete_ExistingProject_ReturnsNoContent()
		{
			// Arrange.
			var projectId = Guid.NewGuid();

			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				// Очитка бд от предыдущих тестов.
				context.Projects.RemoveRange(context.Projects);
				await context.SaveChangesAsync();

				// Добавление тестовых данных в бд.
				context.Projects.Add(new Infrastructure.Entities.ProjectEntity
				{
					Id = projectId,
					Name = "proj 1",
					Description = "desc 1",
					CreatedAt = DateTime.UtcNow
				});
				await context.SaveChangesAsync();
			}

			// Act.
			var response = await _client.DeleteAsync($"/api/projects/{projectId}");

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);

			// Проверка объекта в бд.
			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				var entity = await context.Projects.FindAsync(projectId);
				entity.Should().BeNull();
			}
		}

		[Fact]
		public async SysTask Delete_NoExistingProject_ReturnsNoContent()
		{
			// Arrange.
			var projectId = Guid.NewGuid();

			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				// Очитка бд от предыдущих тестов.
				context.Projects.RemoveRange(context.Projects);
				await context.SaveChangesAsync();
			}

			// Act.
			var response = await _client.DeleteAsync($"/api/projects/{projectId}");

			// Assert.
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);

			// Проверка объекта в бд.
			using (var scope = _applicationFactory.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

				var entity = await context.Projects.FindAsync(projectId);
				entity.Should().BeNull();
			}
		}

		#endregion
	}
}