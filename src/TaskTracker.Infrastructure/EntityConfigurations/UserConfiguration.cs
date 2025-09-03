using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.EntityConfigurations
{
	public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
	{
		public void Configure(EntityTypeBuilder<UserEntity> builder)
		{
			builder.HasKey(u => u.Id);

			builder.Property(u => u.Email);
			builder.Property(u => u.PasswordHash);
			builder.Property(u => u.UserName);
		}
	}
}
