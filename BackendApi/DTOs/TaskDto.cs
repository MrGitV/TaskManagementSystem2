using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs
{
    // Response for task data.
    public class TaskResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Models.TaskStatus Status { get; set; }
        public int? AssignedToUserId { get; set; }
        public int ProjectId { get; set; }
    }

    // Request to create a new task.
    public class CreateTaskRequest
    {
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public Models.TaskStatus Status { get; set; } = Models.TaskStatus.InProgress;
        public int? AssignedToUserId { get; set; }
        public int ProjectId { get; set; }
    }

    // Request to update an existing task.
    public class UpdateTaskRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public Models.TaskStatus Status { get; set; }
        public int? AssignedToUserId { get; set; }
        public int ProjectId { get; set; }
    }
}