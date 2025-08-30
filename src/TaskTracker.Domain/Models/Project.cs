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
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }

		//TODO: Добавить список Tasks.
	}
}
