using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    // Service for managing user-related operations.
    public class UserService(AppDbContext context) : IUserService
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        // Checks if a user with the given username already exists.
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        // Creates a new user.
        public async Task<bool> CreateUserAsync(CreateUserRequest request)
        {
            var user = new Models.User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                Role = Models.UserRole.User
            };
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        // Retrieves all users from the database.
        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.AsNoTracking().Include(u => u.Tasks).ToListAsync();
            return [.. users.Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Role = u.Role.ToString(),
                TaskCount = u.Tasks.Count
            })];
        }

        // Deletes a user by their ID.
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
