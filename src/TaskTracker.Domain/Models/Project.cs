namespace TaskTracker.Domain.Models
{
	/// <summary>
	/// Проект.
	/// </summary>
	public class Project
	{
		public const int MAX_NAME_LENGTH = 128;
		public const int MAX_DESCRIPTION_LENGTH = 1024;

		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }

		public List<Task> Tasks { get; set; } = new();
	}
}
