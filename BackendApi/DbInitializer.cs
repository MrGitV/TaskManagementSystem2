using TaskManagementAPI.Models;

namespace TaskManagementAPI
{
    // A utility class to seed the database with initial data.
    public class DbInitializer
    {
        // Seeds the database if it's empty.
        public static void Seed(AppDbContext context)
        {
            // Seed Users if none exist.
            if (!context.Users.Any())
            {
                var admin = new User { Username = "admin", FullName = "Administrator", Role = UserRole.Admin, PasswordHash = BCrypt.Net.BCrypt.HashPassword("11111") };
                var user1 = new User { Username = "user1", FullName = "User1", Role = UserRole.User, PasswordHash = BCrypt.Net.BCrypt.HashPassword("22222") };
                var user2 = new User { Username = "user2", FullName = "User2", Role = UserRole.User, PasswordHash = BCrypt.Net.BCrypt.HashPassword("33333") };

                context.Users.Add(admin);
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.SaveChanges();
            }

            // Seed Projects if none exist.
            if (!context.Projects.Any())
            {
                var defaultProject = new Project { Title = "Default Project", Description = "General tasks" };
                context.Projects.Add(defaultProject);
                context.SaveChanges();
            }
        }
    }
}