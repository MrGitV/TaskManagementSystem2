namespace ClientApp.DTOs
{
    // Represents the request payload for creating a new comment.
    public class CreateCommentRequest
    {
        public string Text { get; set; }
        public int TaskId { get; set; }
    }
}