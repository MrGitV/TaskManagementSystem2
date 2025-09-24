using Moq;
using TaskManagementAPI.Services;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Tests
{
    public class ProjectServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProjectTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _projectService = new ProjectService(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ReturnsList()
        {
            var projects = new List<Project>
            {
                new() { Id = 1, Title = "Project1" },
                new() { Id = 2, Title = "Project2" }
            };
            _dbContextMock.Setup(c => c.Projects).ReturnsDbSet(projects);

            var result = await _projectService.GetAllProjectsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CreateProjectAsync_ReturnsTrue_WhenCreated()
        {
            var projects = new List<Project>();
            _dbContextMock.Setup(c => c.Projects).ReturnsDbSet(projects);
            _dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var newProjectRequest = new CreateProjectRequest
            {
                Title = "New Project",
                Description = "Description"
            };

            var result = await _projectService.CreateProjectAsync(newProjectRequest);

            Assert.True(result);
            _dbContextMock.Verify(m => m.Projects.Add(It.IsAny<Project>()), Times.Once());
        }
    }
}