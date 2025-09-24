using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);

            var jwtSettings = new JwtSettings
            {
                Secret = "a_very_long_and_secure_secret_key_for_hmac_sha256_testing",
                Issuer = "issuer",
                Audience = "audience",
                ExpirationMinutes = 60
            };

            _authService = new AuthService(_dbContextMock.Object, Options.Create(jwtSettings));
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsToken()
        {
            var user = new User
            {
                Id = 1,
                Username = "test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass"),
                Role = UserRole.User
            };
            var users = new List<User> { user };
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);

            var response = await _authService.AuthenticateAsync("test", "pass");

            Assert.NotNull(response);
            Assert.IsType<LoginResponse>(response);
            Assert.NotEmpty(response.Token!);
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ReturnsNull()
        {
            var user = new User { Id = 1, Username = "test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("wrong") };
            var users = new List<User> { user };
            _dbContextMock.Setup(c => c.Users).ReturnsDbSet(users);

            var response = await _authService.AuthenticateAsync("test", "pass");

            Assert.Null(response);
        }
    }
}