using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskTracker.API.Extensions;
using TaskTracker.API.IntegrationTests.Common;
using TaskTracker.Application.Mapping;
using TaskTracker.Infrastructure.Mapping;
using TaskTracker.Infrastructure.PostgreSqlDb;
using Testcontainers.PostgreSql;

public class DatabaseFixture : IAsyncLifetime
{
	private readonly PostgreSqlContainer _dbContainer =
		new PostgreSqlBuilder()
			.WithDatabase("tasktracker_tests")
			.WithUsername("postgres")
			.WithPassword("notapassword")
			.Build();

	public string ConnectionString => _dbContainer.GetConnectionString();

	public TypeAdapterConfig MapsterTestConfig { get; private set; } = null!;

	public async SysTask InitializeAsync()
	{
		// Запуск контейнера.
		await _dbContainer.StartAsync();

		// Миграции.
		var optionsBuilder = new DbContextOptionsBuilder<TaskTrackerDbContext>()
			.UseNpgsql(ConnectionString);

		using var dbContext = new TaskTrackerDbContext(optionsBuilder.Options);
		await dbContext.Database.MigrateAsync();

		// 3. Настройка отдельного Mapster конфигурационного объекта
		MapsterTestConfig = new TypeAdapterConfig
		{
			RequireDestinationMemberSource = false
		};

		// Сканируем сборки с твоими конфигами
		MapsterTestConfig.Scan(typeof(ProjectDtoModelMappingConfig).Assembly);
		MapsterTestConfig.Scan(typeof(ProjectModelEntityMappingConfig).Assembly);
	}

	public async SysTask DisposeAsync()
	{
		await _dbContainer.StopAsync();
		await _dbContainer.DisposeAsync();
	}

	public TestWebApplicationFactory CreateApplicationFactory()
	{
		return new TestWebApplicationFactory(ConnectionString, MapsterTestConfig);
	}

}
