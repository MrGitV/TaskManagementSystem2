using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // A service for interacting with the projects API endpoint.
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _client;

        // Initializes the service with an HttpClient.
        public ProjectService(HttpClient client)
        {
            _client = client;
        }

        // Retrieves all projects from the API.
        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var response = await _client.GetAsync("/api/projects");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProjectDto>>(json);
        }
    }
}