using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Infrastructure.Entities;

namespace TaskTracker.Infrastructure.EntityConfigurations
{
	public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
	{
		public void Configure(EntityTypeBuilder<TaskEntity> builder)
		{
			builder.HasKey(t => t.Id);

			builder.Property(t => t.Title)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(t => t.Description)
				.IsRequired()
				.HasMaxLength(128);

			builder.Property(t => t.IsCompleted)
				.IsRequired();

			builder.Property(t => t.CreatedAt)
				.IsRequired();

			builder.Property(t => t.UpdatedAt)
				.IsRequired();

			
			builder.HasOne(t => t.ProjectEntity)
				.WithMany(p => p.TaskEntities)
				.HasForeignKey(t => t.ProjectEntityId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
