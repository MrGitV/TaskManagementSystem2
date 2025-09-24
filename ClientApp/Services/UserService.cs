using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // A service for interacting with the users API endpoint.
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        // Initializes the service with an HttpClient.
        public UserService(HttpClient client)
        {
            _client = client;
        }

        // Sends a request to create a new user.
        public async Task<HttpResponseMessage> CreateUserAsync(CreateUserRequest user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/users", content);
            return response;
        }

        // Retrieves all users from the API.
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var response = await _client.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserDto>>(json);
        }

        // Sends a request to delete a user by their ID.
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var response = await _client.DeleteAsync($"/api/users/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}