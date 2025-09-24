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

        // Constructor to set up the mock context and service instance.
        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _userService = new UserService(_dbContextMock.Object);
        }

        [Fact]
        // Tests if the service correctly identifies an existing user.
        public async Task UserExistsAsync_ReturnsTrue_WhenUserExists()
        {
            var users = new List<User> { new() { Id = 1, Username = "user" } };
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);
            var exists = await _userService.UserExistsAsync("user");
            Assert.True(exists);
        }

        [Fact]
        // Tests if the service correctly identifies a non-existent user.
        public async Task UserExistsAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            var users = new List<User>();
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);
            var exists = await _userService.UserExistsAsync("nonexistent");
            Assert.False(exists);
        }

        [Fact]
        // Tests if a new user is successfully added to the database.
        public async Task CreateUserAsync_ReturnsTrue_WhenCreated()
        {
            var users = new List<User>();
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);
            _dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var request = new CreateUserRequest { Username = "newuser", Password = "p", FullName = "n" };
            var result = await _userService.CreateUserAsync(request);
            Assert.True(result);
            _dbContextMock.Verify(m => m.Users.Add(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        // Tests that all users are returned correctly.
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new() { Id = 1, Username = "user1" },
                new() { Id = 2, Username = "user2" }
            };
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);
            var result = await _userService.GetAllUsersAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        // Tests successful deletion of an existing user.
        public async Task DeleteUserAsync_ReturnsTrue_WhenUserExists()
        {
            var user = new User { Id = 1 };
            _dbContextMock.Setup(c => c.Users.FindAsync(1)).ReturnsAsync(user);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var result = await _userService.DeleteUserAsync(1);
            Assert.True(result);
            _dbContextMock.Verify(m => m.Users.Remove(user), Times.Once());
        }

        [Fact]
        // Tests that deletion fails if the user does not exist.
        public async Task DeleteUserAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            _dbContextMock.Setup(c => c.Users.FindAsync(99)).ReturnsAsync((User?)null);
            var result = await _userService.DeleteUserAsync(99);
            Assert.False(result);
        }
    }
}