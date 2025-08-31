using Mapster;
using TaskTracker.Domain.Models;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.Mapping
{
	public class ProjectModelEntityMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// Project <-> ProjectEntity
			TypeAdapterConfig<Project, ProjectEntity>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.Description, src => src.Description)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.Map(dest => dest.TaskEntities, src => src.Tasks)
				.TwoWays()
				.MaxDepth(2);
		}
	}
}
