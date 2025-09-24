using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;
using ApiTask = TaskManagementAPI.Models.Task;

namespace TaskManagementAPI
{
    // Rep data operations.
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        // DbSet for Users table.
        public virtual DbSet<User> Users { get; set; }
        // DbSet for Projects table.
        public virtual DbSet<Project> Projects { get; set; }
        // DbSet for Tasks table.
        public virtual DbSet<ApiTask> Tasks { get; set; }
        // DbSet for Comments table.
        public virtual DbSet<Comment> Comments { get; set; }

        // Configures the model and relationships between entities.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Prevent cascade delete from User to Task.
            modelBuilder.Entity<ApiTask>()
                 .HasOne(t => t.AssignedToUser)
                 .WithMany(u => u.Tasks)
                 .HasForeignKey(t => t.AssignedToUserId)
                 .OnDelete(DeleteBehavior.SetNull);

            // Enable cascade delete from Project to Task.
            modelBuilder.Entity<ApiTask>()
                 .HasOne(t => t.Project)
                 .WithMany(p => p.Tasks)
                 .HasForeignKey(t => t.ProjectId)
                 .OnDelete(DeleteBehavior.Cascade);

            // Prevent cascade delete from User (Author) to Comment.
            modelBuilder.Entity<Comment>()
                 .HasOne(c => c.Author)
                 .WithMany()
                 .HasForeignKey(c => c.AuthorId)
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}