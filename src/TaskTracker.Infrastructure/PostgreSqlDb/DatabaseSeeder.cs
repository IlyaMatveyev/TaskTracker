using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.PostgreSqlDb
{
	public class DatabaseSeeder
	{
		public static async SysTask SeedAsync(TaskTrackerDbContext context, bool hasChanges, CancellationToken cancellationToken)
		{
			if (!await context.Projects.AnyAsync(cancellationToken))
			{
				await SeedProjectsAsync(context, cancellationToken);
			}

			if (!await context.Tasks.AnyAsync(cancellationToken))
			{
				await SeedTasksAsync(context, cancellationToken);
			}
		}

		private static async SysTask SeedProjectsAsync(TaskTrackerDbContext context, CancellationToken cancellationToken)
		{
			var projects = new[]
			{
				new ProjectEntity
				{
					Id = Guid.NewGuid(),
					Name = "Проект 1",
					Description = "Описание 1",
					CreatedAt = DateTime.UtcNow.AddDays(-30)
				},
				new ProjectEntity
				{
					Id = Guid.NewGuid(),
					Name = "Проект 2",
					Description = "Описание 2",
					CreatedAt = DateTime.UtcNow.AddDays(-36)
				},
				new ProjectEntity
				{
					Id = Guid.NewGuid(),
					Name = "Проект 3",
					Description = "Описание 3",
					CreatedAt = DateTime.UtcNow.AddDays(-42)
				},
				new ProjectEntity
				{
					Id = Guid.NewGuid(),
					Name = "Проект 4",
					Description = "Описание 4",
					CreatedAt = DateTime.UtcNow.AddDays(-64)
				},
			};

			await context.Projects.AddRangeAsync(projects, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}

		private static async SysTask SeedTasksAsync(TaskTrackerDbContext context, CancellationToken cancellationToken)
		{
			var projects = await context.Projects.ToListAsync(cancellationToken);
			var tasks = new List<TaskEntity>();
			var random = new Random();

			foreach (var project in projects)
			{
				// Для каждого проекта добавляем 3-5 задач
				var tasksCount = random.Next(3, 6);

				for (int i = 1; i <= tasksCount; i++)
				{
					var isCompleted = random.Next(0, 2) == 1;
					var createdAt = DateTime.UtcNow.AddDays(-random.Next(1, 30));

					tasks.Add(new TaskEntity
					{
						Id = Guid.NewGuid(),
						Title = $"Задача #{i} для {project.Name}",
						Description = $"Описание задачи #{i} для проекта {project.Name}. " +
									 $"Это важная задача, которая требует внимания.",
						IsCompleted = isCompleted,
						ProjectEntityId = project.Id,
						CreatedAt = createdAt,
						UpdatedAt = isCompleted ?
							createdAt.AddDays(random.Next(1, 10)) : 
							createdAt 
					});
				}
			}

			await context.Tasks.AddRangeAsync(tasks, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
	}
}
