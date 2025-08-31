namespace TaskTracker.Application.DTOs
{
	public record TaskUpdateRequest(
		string Title,
		string Description,
		bool IsCompleted,
		Guid ProjectId);
}
