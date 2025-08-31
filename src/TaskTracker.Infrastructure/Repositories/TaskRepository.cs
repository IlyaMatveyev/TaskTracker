using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces;
using TaskTracker.Infrastructure.Entities;
using TaskTracker.Infrastructure.PostgreSqlDb;

namespace TaskTracker.Infrastructure.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly TaskTrackerDbContext _dbContext;
		private readonly IMapper _mapper;

		public TaskRepository(
			TaskTrackerDbContext dbContext, 
			IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<Guid> Create(Task task)
		{
			var taskEntity = _mapper.Map<TaskEntity>(task);

			await _dbContext.Tasks.AddAsync(taskEntity);
			await _dbContext.SaveChangesAsync();

			return taskEntity.Id;
		}

		public async Task<Task> GetById(Guid taskId)
		{
			var taskEntity = await _dbContext.Tasks
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == taskId);

			if (taskEntity == null)
			{
				throw new KeyNotFoundException("Task not found.");
			}

			var task = _mapper.Map<Task>(taskEntity);

			return task;
		}

		public async Task<List<Task>> GetAll(bool? isCompleted, Guid? projectId)
		{
			var query = _dbContext.Tasks.AsQueryable();

			if (isCompleted.HasValue)
			{
				query = query.Where(t => t.IsCompleted == isCompleted.Value);
			}

			if (projectId.HasValue)
			{
				query = query.Where(t => t.ProjectEntityId == projectId.Value);
			}

			var taskEntityList = await query.ToListAsync();

			return _mapper.Map<List<Task>>(taskEntityList);
		}
	}
}
