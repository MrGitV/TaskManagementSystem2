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

        // Constructor to set up the mock context and service instance.
        public ProjectServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProjectTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _projectService = new ProjectService(_dbContextMock.Object);
        }

        [Fact]
        // Tests retrieving all projects.
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
        // Tests the successful creation of a new project.
        public async Task CreateProjectAsync_ReturnsTrue_WhenCreated()
        {
            var projects = new List<Project>();
            _dbContextMock.Setup(c => c.Projects).ReturnsDbSet(projects);
            _dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var newProjectRequest = new CreateProjectRequest { Title = "New Project" };
            var result = await _projectService.CreateProjectAsync(newProjectRequest);
            Assert.True(result);
            _dbContextMock.Verify(m => m.Projects.Add(It.Is<Project>(p => p.Title == "New Project")), Times.Once());
        }

        [Fact]
        // Tests successful deletion of an existing project.
        public async Task DeleteProjectAsync_ReturnsTrue_WhenProjectExists()
        {
            var project = new Project { Id = 1, Title = "ProjectToDelete" };
            _dbContextMock.Setup(c => c.Projects.FindAsync(1)).ReturnsAsync(project);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var result = await _projectService.DeleteProjectAsync(1);
            Assert.True(result);
            _dbContextMock.Verify(m => m.Projects.Remove(project), Times.Once());
        }

        [Fact]
        // Tests that deletion fails if the project does not exist.
        public async Task DeleteProjectAsync_ReturnsFalse_WhenProjectDoesNotExist()
        {
            _dbContextMock.Setup(c => c.Projects.FindAsync(99)).ReturnsAsync((Project?)null);
            var result = await _projectService.DeleteProjectAsync(99);
            Assert.False(result);
            _dbContextMock.Verify(m => m.Projects.Remove(It.IsAny<Project>()), Times.Never());
        }
    }
}