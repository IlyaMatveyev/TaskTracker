using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskTracker.Infrastructure.Auth;

namespace TaskTracker.API.Extensions
{
	public static class Auth
	{
		public static void AddAuth(this IServiceCollection services, IOptions<JwtOptions> jwtOptions)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
				{
					options.TokenValidationParameters = new()
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,

						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
					};

					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							context.Token = context.Request.Cookies["token"];

							return SysTask.CompletedTask;
						}
					};
				});

			services.AddAuthentication();
		}
	}
}
