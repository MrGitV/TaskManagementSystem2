using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs
{
    // Response for comment data.
    public class CommentResponse
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        public int? AuthorId { get; set; }
        public int TaskId { get; set; }
        public UserResponse? Author { get; set; }
    }

    // Request to add a comment.
    public class CreateCommentRequest
    {
        [Required]
        public string? Text { get; set; }

        public int TaskId { get; set; }
    }
}