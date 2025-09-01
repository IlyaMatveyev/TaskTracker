using Microsoft.EntityFrameworkCore;
using TaskTracker.API.Extensions;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.Services;
using TaskTracker.Infrastructure.HostedServices;
using TaskTracker.Infrastructure.PostgreSqlDb;
using TaskTracker.Infrastructure.Repositories;
using Serilog;
using TaskTracker.API.Exceptions;

namespace TaskTracker.API
{
    public class Program
    {
        public static async SysTask Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Конфигурирование логгера.
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.CreateLogger();

            builder.Host.UseSerilog();

			builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TaskTrackerDbContext>(
                options =>
                {
                    options.UseNpgsql(
                        builder.Configuration.GetConnectionString(nameof(TaskTrackerDbContext)), 
                        b => b.MigrationsAssembly(typeof(TaskTrackerDbContext).Assembly.FullName)
                    );
                }
            );

            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
			builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            // Инициализатор базы данных.
            builder.Services.AddHostedService<DatabaseInitializationService>();

            // Добавление маппинга.
            builder.Services.RegisterMapsterConfiguration();

            // Регистрация обработчика исключений.
            builder.Services.AddExceptionHandler<ExceptionHandler>();

            // Регистрация валидаторов FluentValidation.
            builder.Services.RegisterFluentValidationConfig();

			var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Встраиваем обработчик исключений в конвейер.
            app.UseExceptionHandler(_ => { });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
