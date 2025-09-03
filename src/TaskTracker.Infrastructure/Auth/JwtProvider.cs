using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Models;

namespace TaskTracker.Infrastructure.Auth
{
	public class JwtProvider : IJwtProvider
	{
		private readonly JwtOptions _jwtOptions;

		public JwtProvider(IOptions<JwtOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;
		}

		public string GenerateToken(User user)
		{
			Claim[] claims = { new("userId", user.Id.ToString()) };

			// Настройка алгоритма.
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
				SecurityAlgorithms.HmacSha256);

			// Создание токена.
			var token = new JwtSecurityToken(
				claims: claims,
				signingCredentials: signingCredentials,
				expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));

			var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

			return tokenValue;
		}
	}
}
