using MapsterMapper;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.Application.Services
{
	public class TaskService : ITaskService
	{
		private readonly ITaskRepository _taskRepository;
		private readonly IMapper _mapper;
		public TaskService(ITaskRepository taskRepository, IMapper mapper)
		{
			_taskRepository = taskRepository;
			_mapper = mapper;
		}

		public async Task<Guid> Create(TaskCreateRequest taskCreate)
		{
			var task = _mapper.Map<Task>(taskCreate);

			return await _taskRepository.Create(task);
		}
	}
}
