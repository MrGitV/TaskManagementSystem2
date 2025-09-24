using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // Defines the contract for a service that manages tasks and comments.
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<List<TaskDto>> GetAllTasksForDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<TaskDto>> GetTasksForUserAsync(string userId, DateTime startDate, DateTime endDate);
        Task UpdateTaskAsync(TaskDto task);
        Task<int> AddTaskAsync(CreateTaskRequest task);
        Task<bool> DeleteTaskAsync(int taskId);
        Task AddCommentAsync(CreateCommentRequest comment);
        Task<List<CommentDto>> GetCommentsForTaskAsync(int taskId);
    }
}