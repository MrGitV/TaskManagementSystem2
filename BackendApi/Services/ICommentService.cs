using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    // Interface for comment CRUD.
    public interface ICommentService
    {
        Task<List<CommentResponse>> GetCommentsForTaskAsync(int taskId);
        Task<bool> AddCommentAsync(CreateCommentRequest request, int authorId);
    }
}