using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    // Interface for user CRUD operations.
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string username);
        Task<bool> CreateUserAsync(CreateUserRequest request);
        Task<List<UserResponse>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
    }
}