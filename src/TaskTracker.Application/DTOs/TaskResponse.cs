namespace TaskTracker.Application.DTOs
{
	public record TaskResponse(
		Guid Id,
		string Title,
		string Description,
		bool IsCompleted,
		DateTime CreatedAt,
		DateTime UpdatedAt,
		Guid ProjectId);
}
