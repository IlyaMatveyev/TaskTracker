using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Interfaces
{
	public interface IUserService
	{
		public Task<Guid> Register(UserRegisterRequest registerRequest);
		public Task<string> Login(UserLoginRequest registerRequest);
	}
}
