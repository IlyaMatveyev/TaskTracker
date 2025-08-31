namespace TaskTracker.Application.DTOs
{
	public record ProjectWithTasksResponse(
		Guid Id,
		string Name,
		string Description,
		DateTime CreatedAt,
		List<TaskResponse> Tasks);
}
