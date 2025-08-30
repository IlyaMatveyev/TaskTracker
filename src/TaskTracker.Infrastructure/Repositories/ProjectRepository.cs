using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Models;
using TaskTracker.Infrastructure.PostgreSqlDb;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.Infrastructure.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly TaskTrackerDbContext _dbContext;
		public ProjectRepository(TaskTrackerDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Project>> GetAll()
		{
			var projectEntityList = await _dbContext.Projects
				.AsNoTracking()
				.ToListAsync();

			var projectList = projectEntityList
				.Select(p => new Project
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					CreatedAt = p.CreatedAt
					//TODO: Tasks
				}).ToList();

			return projectList;
		}


		public async Task<Guid> Create(Project project)
		{
			await _dbContext.Projects.AddAsync(new()
			{
				Id = project.Id,
				Name = project.Name,
				Description = project.Description,
				CreatedAt = project.CreatedAt
			});

			await _dbContext.SaveChangesAsync();

			return project.Id;
		}

		public async Task<int> Delete(Guid id)
		{
			var deletedRowsCount = await _dbContext.Projects
				.Where(p => p.Id == id)
				.ExecuteDeleteAsync();

			return deletedRowsCount;
		}

		public async Task<Guid> Update(Guid id, Project project)
		{
			await _dbContext.Projects
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(setPropCalls => setPropCalls
					.SetProperty(p => p.Name, p => project.Name)
					.SetProperty(p => p.Description, p => project.Description)
					.SetProperty(p => p.CreatedAt, p => project.CreatedAt)
					//TODO: Tasks
					);

			return id;
		}
	}
}
