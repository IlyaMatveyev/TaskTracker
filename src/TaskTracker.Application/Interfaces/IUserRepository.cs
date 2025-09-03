using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Interfaces
{
	public interface IUserRepository
	{
		public Task<Guid> Create(User user);
		public Task<User> GetByEmail(string email);
		public Task<bool> CheckEmailUnique(string email);
	}
}
