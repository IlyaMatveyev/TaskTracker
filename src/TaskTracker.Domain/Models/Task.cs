namespace TaskTracker.Domain.Models
{
	public class Task
	{
		public const int MIN_TITLE_LENGTH = 1;
		public const int MAX_TITLE_LENGTH = 50;

		public const int MAX_DESCRIPTION_LENGTH = 128;

		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsCompleted { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Guid ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
