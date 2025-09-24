using System;

namespace ClientApp.DTOs
{
    // Represents a task data transfer object.
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public int? AssignedToUserId { get; set; }
        public int ProjectId { get; set; }
    }
}