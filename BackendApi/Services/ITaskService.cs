using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    // Interface for task CRUD.
    public interface ITaskService
    {
        Task<List<TaskResponse>> GetAllTasksAsync(DateTime? startDate, DateTime? endDate);
        Task<List<TaskResponse>> GetTasksForUserAsync(int userId, DateTime startDate, DateTime endDate);
        Task<TaskResponse?> GetTaskByIdAsync(int taskId);
        Task<bool> UpdateTaskAsync(UpdateTaskRequest request);
        Task<int> AddTaskAsync(CreateTaskRequest request);
        Task<bool> DeleteTaskAsync(int taskId);
    }
}