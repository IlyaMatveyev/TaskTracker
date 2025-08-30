using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Models;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.EntityConfigurations
{
	public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
	{
		public void Configure(EntityTypeBuilder<ProjectEntity> builder)
		{
			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(Project.MAX_NAME_LENGTH);

			builder.Property(p => p.Description)
				.IsRequired()
				.HasMaxLength(Project.MAX_DESCRIPTION_LENGTH);

			builder.Property(p => p.CreatedAt)
				.IsRequired();
		}
	}
}
