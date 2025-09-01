using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Models;
using TaskTracker.Infrastructure.PostgreSqlDb;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.DTOs.Common;
using MapsterMapper;
using Mapster;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly TaskTrackerDbContext _dbContext;
		private readonly IMapper _mapper;

		public ProjectRepository(
			TaskTrackerDbContext dbContext, 
			IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<PagedResponse<Project>> GetAll(PaginationParams paginationParams)
		{
			var query = _dbContext.Projects
				.AsNoTracking()
				.OrderBy(p => p.CreatedAt);

			var totalCount = await query.CountAsync();

			var projectEntityList = await query
				.Skip((paginationParams.Page - 1) * paginationParams.PageSize)
				.Take(paginationParams.PageSize)
				.ToListAsync();

			var pagedResponse = new PagedResponse<Project>()
			{
				Items = projectEntityList.Adapt<List<Project>>(),
				Page = paginationParams.Page,
				PageSize = paginationParams.PageSize,
				TotalCount = totalCount
			};

			return pagedResponse;
		}

		public async Task<Project> GetById(Guid projectId)
		{
			var projectEntity = await _dbContext.Projects
				.Include(p => p.TaskEntities)
				.FirstOrDefaultAsync(p => p.Id == projectId);

			if (projectEntity == null)
			{
				throw new KeyNotFoundException("Project not found.");
			}

			var project = _mapper.Map<Project>(projectEntity);

			return project;
		}

		public async Task<Guid> Create(Project project)
		{
			var projectEntity = _mapper.Map<ProjectEntity>(project);

			await _dbContext.Projects.AddAsync(projectEntity);

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
			var updatedRowsCount = await _dbContext.Projects
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(setPropCalls => setPropCalls
					.SetProperty(p => p.Name, p => project.Name)
					.SetProperty(p => p.Description, p => project.Description)
					);

			if(updatedRowsCount < 1)
			{
				throw new KeyNotFoundException("Project not found.");
			}

			return id;
		}
	}
}
