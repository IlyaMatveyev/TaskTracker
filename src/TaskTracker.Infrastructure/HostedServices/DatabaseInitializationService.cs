using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskTracker.Infrastructure.PostgreSqlDb;

namespace TaskTracker.Infrastructure.HostedServices
{
	public class DatabaseInitializationService : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<DatabaseInitializationService> _logger;

		public DatabaseInitializationService(
			IServiceProvider serviceProvider, 
			ILogger<DatabaseInitializationService> logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			using var scope = _serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();

			try
			{
				_logger.LogInformation("Applying database migrations...");

				await context.Database.MigrateAsync();

				_logger.LogInformation("The database migrations were applied successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred when applying DATABASE migrations.");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Database initialization service stopped.");
			
			return Task.CompletedTask;
		}
	}
}
