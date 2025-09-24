using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;
using ApiTask = TaskManagementAPI.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _taskService = new TaskService(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsList()
        {
            var tasks = new List<ApiTask>
            {
                new() { Id = 1, Title = "Task1" },
                new() { Id = 2, Title = "Task2" }
            };
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);

            var result = await _taskService.GetAllTasksAsync(null, null);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddTaskAsync_ReturnsNewId()
        {
            var tasks = new List<ApiTask>();
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var request = new CreateTaskRequest
            {
                Title = "New Task",
                Description = "Desc",
                DueDate = DateTime.Now.AddDays(1),
                Status = Models.TaskStatus.InProgress,
                ProjectId = 1
            };

            var newId = await _taskService.AddTaskAsync(request);

            _dbContextMock.Verify(m => m.Tasks.Add(It.IsAny<ApiTask>()), Times.Once());
            Assert.True(newId >= 0);
        }

        [Fact]
        public async Task UpdateTaskAsync_ReturnsTrue_WhenUpdated()
        {
            var taskInDb = new ApiTask { Id = 1, Title = "Old" };
            var tasks = new List<ApiTask> { taskInDb };

            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            _dbContextMock.Setup(c => c.Tasks.FindAsync(1)).ReturnsAsync(taskInDb);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var request = new UpdateTaskRequest
            {
                Id = 1,
                Title = "New Title",
                Description = "Desc",
                DueDate = DateTime.Now.AddDays(2),
                Status = Models.TaskStatus.Completed,
                ProjectId = 1
            };

            var result = await _taskService.UpdateTaskAsync(request);

            Assert.True(result);
            Assert.Equal("New Title", taskInDb.Title);
        }
    }
}