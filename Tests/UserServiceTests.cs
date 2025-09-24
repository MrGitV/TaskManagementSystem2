using Moq;
using TaskManagementAPI.Services;
using TaskManagementAPI.Models;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _userService = new UserService(_dbContextMock.Object);
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsTrue_WhenUserExists()
        {
            var users = new List<User> { new() { Id = 1, Username = "user" } };
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);

            var exists = await _userService.UserExistsAsync("user");

            Assert.True(exists);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsTrue_WhenCreated()
        {
            var users = new List<User>();
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);
            _dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var createUserRequest = new CreateUserRequest
            {
                Username = "newuser",
                Password = "pass",
                FullName = "New User"
            };

            var result = await _userService.CreateUserAsync(createUserRequest);

            Assert.True(result);
            _dbContextMock.Verify(m => m.Users.Add(It.IsAny<User>()), Times.Once());
        }
    }
}