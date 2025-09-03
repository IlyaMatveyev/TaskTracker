using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Entities;
using TaskTracker.Infrastructure.EntityConfigurations;

namespace TaskTracker.Infrastructure.PostgreSqlDb
{
	public class TaskTrackerDbContext : DbContext
	{
		public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) 
			: base(options)
		{
		}

		public DbSet<ProjectEntity> Projects { get; set; }
		public DbSet<TaskEntity> Tasks { get; set; }
		public DbSet<UserEntity> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ProjectConfiguration());
			modelBuilder.ApplyConfiguration(new TaskConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
