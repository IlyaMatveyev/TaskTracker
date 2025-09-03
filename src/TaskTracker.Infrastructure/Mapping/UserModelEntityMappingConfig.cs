using Mapster;
using TaskTracker.Domain.Models;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.Mapping
{
	public class UserModelEntityMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// User -> UserEntity
			TypeAdapterConfig<User, UserEntity>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Email, src => src.Email)
				.Map(dest => dest.PasswordHash, src => src.PasswordHash)
				.Map(dest => dest.UserName, src => src.UserName);

			// UserEntity -> User
			TypeAdapterConfig<UserEntity, User>
				.NewConfig()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Email, src => src.Email)
				.Map(dest => dest.PasswordHash, src => src.PasswordHash)
				.Map(dest => dest.UserName, src => src.UserName);
		}
	}
}
