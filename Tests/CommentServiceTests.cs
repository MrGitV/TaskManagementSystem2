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

        public CommentServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CommentTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _commentService = new CommentService(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetCommentsForTaskAsync_ReturnsList()
        {
            var comments = new List<Comment>
            {
                new() { Id = 1, Text = "Comment1", TaskId = 1, Author = new User { FullName = "Test" } },
                new() { Id = 2, Text = "Comment2", TaskId = 1, Author = new User { FullName = "Test" } }
            };
            _dbContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);

            var result = await _commentService.GetCommentsForTaskAsync(1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddCommentAsync_ReturnsTrue_WhenAdded()
        {
            var comments = new List<Comment>();
            _dbContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var request = new CreateCommentRequest
            {
                Text = "New Comment",
                TaskId = 1
            };
            var authorId = 1;

            var result = await _commentService.AddCommentAsync(request, authorId);

            Assert.True(result);
            _dbContextMock.Verify(m => m.Comments.Add(It.IsAny<Comment>()), Times.Once());
        }
    }
}