using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using Task = AppCore.Entities.Task;

namespace Infrastructure.DataContext;

public class TaskContext : DbContext
{
    public TaskContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Task> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().HasMany(p => p.Tasks)
            .WithOne().OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }

}