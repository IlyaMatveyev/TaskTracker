namespace TaskTracker.Infrastructure.Entities
{
	public class TaskEntity
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsCompleted { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Guid ProjectEntityId { get; set; }
		public ProjectEntity ProjectEntity { get; set; }
	}
}
