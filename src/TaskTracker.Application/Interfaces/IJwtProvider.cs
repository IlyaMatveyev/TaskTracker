using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Interfaces
{
	public interface IJwtProvider
	{
		string GenerateToken(User user);
	}
}
