using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    // Service for managing comment-related operations.
    public class CommentService(AppDbContext context) : ICommentService
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        // Retrieves all comments for a given task.
        public async Task<List<CommentResponse>> GetCommentsForTaskAsync(int taskId)
        {
            var comments = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Author)
                .Where(c => c.TaskId == taskId)
                .OrderBy(c => c.SentAt)
                .ToListAsync();
            return [.. comments.Select(c => new CommentResponse
            {
                Id = c.Id,
                Text = c.Text,
                SentAt = c.SentAt,
                AuthorId = c.AuthorId,
                TaskId = c.TaskId,
                Author = c.Author == null ? null : new UserResponse
                {
                    Id = c.Author.Id,
                    Username = c.Author.Username,
                    FullName = c.Author.FullName,
                    Role = c.Author.Role.ToString()
                }
            })];
        }

        // Adds a new comment to a task.
        public async Task<bool> AddCommentAsync(CreateCommentRequest request, int authorId)
        {
            var comment = new Comment
            {
                Text = request.Text,
                SentAt = DateTime.UtcNow,
                AuthorId = authorId,
                TaskId = request.TaskId
            };
            _context.Comments.Add(comment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}