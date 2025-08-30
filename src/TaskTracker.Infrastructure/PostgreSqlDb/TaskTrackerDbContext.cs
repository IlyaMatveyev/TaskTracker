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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ProjectConfiguration());
			//TODO: Далее тут применить конфигурацию Task.

			base.OnModelCreating(modelBuilder);
		}
	}
}
