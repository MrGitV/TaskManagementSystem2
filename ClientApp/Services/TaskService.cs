using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // A service for interacting with the tasks and comments API endpoints.
    public class TaskService : ITaskService
    {
        private readonly HttpClient _client;

        // Private class to deserialize the ID from the add task response.
        private class AddTaskResponse { public int Id { get; set; } }

        // Initializes the service with an HttpClient.
        public TaskService(HttpClient client)
        {
            _client = client;
        }

        // Retrieves all tasks from the API.
        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var response = await _client.GetAsync("/api/tasks");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TaskDto>>(json);
        }

        // Sends an update request for a specific task.
        public async Task UpdateTaskAsync(TaskDto task)
        {
            var content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/tasks/{task.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        // Sends a request to create a new task.
        public async Task<int> AddTaskAsync(CreateTaskRequest task)
        {
            var content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/tasks", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AddTaskResponse>(json);
            return result.Id;
        }

        // Sends a request to delete a task by its ID.
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var response = await _client.DeleteAsync($"/api/tasks/{taskId}");
            return response.IsSuccessStatusCode;
        }

        // Sends a request to add a new comment to a task.
        public async Task AddCommentAsync(CreateCommentRequest comment)
        {
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/comments", content);
            response.EnsureSuccessStatusCode();
        }

        // Retrieves all comments for a specific task.
        public async Task<List<CommentDto>> GetCommentsForTaskAsync(int taskId)
        {
            var response = await _client.GetAsync($"/api/comments/{taskId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CommentDto>>(json);
        }

        // Retrieves all tasks within a given date range.
        public async Task<List<TaskDto>> GetAllTasksForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var formattedStartDate = startDate.ToString("o");
            var formattedEndDate = endDate.ToString("o");

            var uri = $"/api/tasks?startDate={Uri.EscapeDataString(formattedStartDate)}&endDate={Uri.EscapeDataString(formattedEndDate)}";

            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TaskDto>>(json);
        }


        // Retrieves all tasks for a specific user within a date range.
        public async Task<List<TaskDto>> GetTasksForUserAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var formattedStartDate = startDate.ToString("yyyy-MM-dd");
            var formattedEndDate = endDate.ToString("yyyy-MM-dd");

            var uri = $"/api/tasks/user/{userId}?startDate={Uri.EscapeDataString(formattedStartDate)}&endDate={Uri.EscapeDataString(formattedEndDate)}";

            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TaskDto>>(json);
        }
    }
}