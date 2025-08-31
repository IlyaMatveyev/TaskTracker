using Mapster;
using MapsterMapper;
using TaskTracker.Application.Mapping;
using TaskTracker.Infrastructure.Mapping;

namespace TaskTracker.API.Extensions
{
	public static class MapsterConfig
	{
		public static void RegisterMapsterConfiguration(this IServiceCollection services)
		{
			var config = TypeAdapterConfig.GlobalSettings;

			// Сканим сборку на наличие конфигов маппинга.
			config.Scan(typeof(ProjectDtoModelMappingConfig).Assembly);
			config.Scan(typeof(ProjectModelEntityMappingConfig).Assembly);

			// Регистрация конфигураций и IMapper в DI
			services.AddSingleton(config);
			services.AddScoped<IMapper, ServiceMapper>();
		}
	}
}
