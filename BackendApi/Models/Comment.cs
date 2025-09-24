using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models
{
    // Represents a comment made on a task.
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Text { get; set; }

        public DateTime SentAt { get; set; }
        public int? AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public virtual User? Author { get; set; }

        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public virtual Task? Task { get; set; }
    }
}