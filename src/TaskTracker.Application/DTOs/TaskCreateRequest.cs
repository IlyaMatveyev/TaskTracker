namespace TaskTracker.Application.DTOs
{
	public record TaskCreateRequest(
		string Title, 
		string Description,
		bool IsCompleted,
		Guid ProjectId);
}
