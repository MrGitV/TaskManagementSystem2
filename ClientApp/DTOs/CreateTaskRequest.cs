using System;

namespace ClientApp.DTOs
{
    // Represents the request payload for creating a new task.
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.InProgress;
        public int? AssignedToUserId { get; set; }
        public int ProjectId { get; set; }
    }
}