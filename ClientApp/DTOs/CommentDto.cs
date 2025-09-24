using System;

namespace ClientApp.DTOs
{
    // Represents a comment data transfer object.
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public int? AuthorId { get; set; }
        public int TaskId { get; set; }
        public UserDto Author { get; set; }
    }
}