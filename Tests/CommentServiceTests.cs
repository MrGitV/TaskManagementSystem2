using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Tests
{
    public class CommentServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly CommentService _commentService;

        // Constructor to set up the mock context and service instance.
        public CommentServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CommentTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _commentService = new CommentService(_dbContextMock.Object);
        }

        [Fact]
        // Tests fetching all comments for a specific task.
        public async Task GetCommentsForTaskAsync_ReturnsMatchingComments()
        {
            var comments = new List<Comment>
            {
                new() { Id = 1, Text = "Comment1", TaskId = 1, Author = new User() },
                new() { Id = 2, Text = "Comment2", TaskId = 1, Author = new User() },
                new() { Id = 3, Text = "Other Task Comment", TaskId = 2, Author = new User() }
            };
            _dbContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
            var result = await _commentService.GetCommentsForTaskAsync(1);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal(1, c.TaskId));
        }

        [Fact]
        // Tests that an empty list is returned for a task with no comments.
        public async Task GetCommentsForTaskAsync_ReturnsEmptyList_WhenTaskHasNoComments()
        {
            var comments = new List<Comment>();
            _dbContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
            var result = await _commentService.GetCommentsForTaskAsync(99);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        // Tests the successful creation of a new comment.
        public async Task AddCommentAsync_ReturnsTrue_OnSuccessfulSave()
        {
            var comments = new List<Comment>();
            _dbContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var request = new CreateCommentRequest { Text = "New Comment", TaskId = 1 };
            var result = await _commentService.AddCommentAsync(request, 1);
            Assert.True(result);
            _dbContextMock.Verify(m => m.Comments.Add(It.IsAny<Comment>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        // Tests the case where saving changes to the database fails.
        public async Task AddCommentAsync_ReturnsFalse_WhenSaveChangesFails()
        {
            var comments = new List<Comment>();
            _dbContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);
            var request = new CreateCommentRequest { Text = "New Comment", TaskId = 1 };
            var result = await _commentService.AddCommentAsync(request, 1);
            Assert.False(result);
        }
    }
}