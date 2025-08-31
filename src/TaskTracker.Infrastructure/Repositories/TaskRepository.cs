using MapsterMapper;
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
	}
}
