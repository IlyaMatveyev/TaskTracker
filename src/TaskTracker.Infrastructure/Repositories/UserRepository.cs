using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Models;
using TaskTracker.Infrastructure.Entities;
using TaskTracker.Infrastructure.PostgreSqlDb;

namespace TaskTracker.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly TaskTrackerDbContext _dbContext;
		private readonly IMapper _mapper;

		public UserRepository(
			TaskTrackerDbContext dbContext, 
			IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<Guid> Create(User user)
		{
			var userEntity = _mapper.Map<UserEntity>(user);

			await _dbContext.Users.AddAsync(userEntity);
			await _dbContext.SaveChangesAsync();

			return userEntity.Id;
		}

		public async Task<User> GetByEmail(string email)
		{
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Email == email);

			if (userEntity == null)
			{
				throw new KeyNotFoundException("User not found.");
			}
			var user = _mapper.Map<User>(userEntity);
			return user;
		}

		public async Task<bool> CheckEmailUnique(string email)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

			return user == null;
		}
	}
}
