using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Models;

namespace TaskTracker.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IJwtProvider _jwtProvider;

		public UserService(
			IUserRepository userRepository, 
			IPasswordHasher passwordHasher, 
			IJwtProvider jwtProvider)
		{
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
			_jwtProvider = jwtProvider;
		}

		public async Task<Guid> Register(UserRegisterRequest registerRequest)
		{
			var isEmailUnique = await _userRepository.CheckEmailUnique(registerRequest.Email);

			if (!isEmailUnique)
			{
				return Guid.Empty;
			}

			var hashedPassword = _passwordHasher.Generate(registerRequest.Password);
			var user = new User(registerRequest.UserName, hashedPassword, registerRequest.Email);

			return await _userRepository.Create(user);
		}

		public async Task<string> Login(UserLoginRequest loginRequest)
		{
			var userFromDb = await _userRepository.GetByEmail(loginRequest.Email);

			if(userFromDb == null ||
				!_passwordHasher.Verify(loginRequest.Password, userFromDb.PasswordHash))
			{
				throw new KeyNotFoundException("Wrong email or password.");
			}

			// Генерация токена.
			var jwtToken = _jwtProvider.GenerateToken(userFromDb);

			return jwtToken;
		}
	}
}
