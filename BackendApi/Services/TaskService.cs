using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using ApiTask = TaskManagementAPI.Models.Task;

namespace TaskManagementAPI.Services
{
    // Service for managing task-related operations.
    public class TaskService(AppDbContext context) : ITaskService
    {
        private readonly AppDbContext _context = context;

        // Gets all tasks, optionally filtered by a date range.
        public async Task<List<TaskResponse>> GetAllTasksAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Tasks.AsNoTracking();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date >= startDate.Value.Date && t.DueDate.Date <= endDate.Value.Date);
            }

            return await query.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreationDate = t.CreationDate,
                DueDate = t.DueDate,
                Status = t.Status,
                AssignedToUserId = t.AssignedToUserId,
                ProjectId = t.ProjectId
            })
                .ToListAsync();
        }

        // Gets tasks for a specific user within a date range.
        public async Task<List<TaskResponse>> GetTasksForUserAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedToUserId == userId && t.DueDate.Date >= startDate.Date && t.DueDate.Date <= endDate.Date)
                .Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreationDate = t.CreationDate,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    AssignedToUserId = t.AssignedToUserId,
                    ProjectId = t.ProjectId
                })
                .ToListAsync();
        }

        // Gets a task by its ID.
        public async Task<TaskResponse?> GetTaskByIdAsync(int taskId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.Id == taskId)
                .Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreationDate = t.CreationDate,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    AssignedToUserId = t.AssignedToUserId,
                    ProjectId = t.ProjectId
                })
                .FirstOrDefaultAsync();
        }

        // Updates an existing task.
        public async Task<bool> UpdateTaskAsync(UpdateTaskRequest request)
        {
            var task = await _context.Tasks.FindAsync(request.Id);
            if (task == null) return false;

            task.Title = request.Title;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.Status = request.Status;
            task.AssignedToUserId = request.AssignedToUserId;
            task.ProjectId = request.ProjectId;

            return await _context.SaveChangesAsync() > 0;
        }

        // Adds a new task to the database.
        public async Task<int> AddTaskAsync(CreateTaskRequest request)
        {
            var task = new ApiTask
            {
                Title = request.Title,
                Description = request.Description,
                CreationDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                Status = request.Status,
                AssignedToUserId = request.AssignedToUserId,
                ProjectId = request.ProjectId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task.Id;
        }

        // Deletes a task by its ID.
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;
            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}