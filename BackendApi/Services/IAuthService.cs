using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    // Interface for authentication service.
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(string username, string password);
    }
}