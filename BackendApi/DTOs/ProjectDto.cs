using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs
{
    // Response for project data.
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    // Request to create a new project.
    public class CreateProjectRequest
    {
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}