using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    // Represents a project that contains multiple tasks.
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }
        public virtual ICollection<Task> Tasks { get; set; } = [];
    }
}