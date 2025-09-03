namespace TaskTracker.Application.DTOs
{
	public record UserRegisterRequest(string Email, string Password, string UserName);
}
