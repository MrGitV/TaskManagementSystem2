using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // Defines the contract for a service that manages users.
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<HttpResponseMessage> CreateUserAsync(CreateUserRequest user);
        Task<bool> DeleteUserAsync(int userId);
    }
}