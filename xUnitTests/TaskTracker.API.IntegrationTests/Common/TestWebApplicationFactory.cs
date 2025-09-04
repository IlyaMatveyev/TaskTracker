using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskTracker.Infrastructure.PostgreSqlDb;

namespace TaskTracker.API.IntegrationTests.Common
{
	public class TestWebApplicationFactory : WebApplicationFactory<Program>
	{
		private readonly string _connectionString;
		private readonly TypeAdapterConfig _mapsterConfig;

		public TestWebApplicationFactory(string connectionString, TypeAdapterConfig mapsterConfig)
		{
			_connectionString = connectionString;
			_mapsterConfig = mapsterConfig;
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// Убираем старый DbContext.
				services.RemoveAll<DbContextOptions<TaskTrackerDbContext>>();
				services.RemoveAll<TaskTrackerDbContext>();

				// Регистрация тестового DbContext с контейнером.
				services.AddDbContext<TaskTrackerDbContext>(options =>
					options.UseNpgsql(_connectionString));

				// Регистрируем отдельную конфигурацию Mapster для тестов
				services.AddSingleton(_mapsterConfig);
				services.AddScoped<IMapper, ServiceMapper>();
			});
		}
	}
}
