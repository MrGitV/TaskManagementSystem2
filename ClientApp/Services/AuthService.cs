using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // API service for authentication.
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private string _token;
        private int? _userId;
        private string _role;

        // Service constructor.
        public AuthService(HttpClient client)
        {
            _client = client;
        }

        // Logs in the user and stores session data.
        public async Task<bool> LoginAsync(string username, string password)
        {
            var request = new { Username = username, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync("/api/auth/login", content);
                if (!response.IsSuccessStatusCode)
                {
                    Logout();
                    return false;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResponse>(json);
                _token = result.Token;
                _userId = result.UserId;
                _role = result.Role;
                return true;
            }
            catch (HttpRequestException)
            {
                Logout();
                return false;
            }
        }

        // Clears all stored session data.
        public void Logout() => (_token, _userId, _role) = (null, null, null);

        // Gets the current user's authentication token.
        public string GetCurrentToken() => _token;
        // Gets the current user's ID.
        public int? CurrentUserId => _userId;
        // Gets the current user's role.
        public string CurrentRole => _role;
    }
}