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

        // Constructor to set up the mock context and service instance.
        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskTestDb")
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _taskService = new TaskService(_dbContextMock.Object);
        }

        [Fact]
        // Tests retrieving all tasks.
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            var tasks = new List<ApiTask> { new() { Id = 1 }, new() { Id = 2 } };
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            var result = await _taskService.GetAllTasksAsync(null, null);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        // Tests retrieving an empty list when no tasks exist.
        public async Task GetAllTasksAsync_ReturnsEmptyList_WhenNoTasksExist()
        {
            var tasks = new List<ApiTask>();
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            var result = await _taskService.GetAllTasksAsync(null, null);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        // Tests fetching a single task by its ID.
        public async Task GetTaskByIdAsync_ReturnsTask_WhenTaskExists()
        {
            var tasks = new List<ApiTask> { new() { Id = 1, Title = "Found Task" } };
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            var result = await _taskService.GetTaskByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        // Tests fetching a non-existent task.
        public async Task GetTaskByIdAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            var tasks = new List<ApiTask>();
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            var result = await _taskService.GetTaskByIdAsync(99);
            Assert.Null(result);
        }

        [Fact]
        // Tests the successful creation of a new task.
        public async Task AddTaskAsync_ReturnsNewId()
        {
            var tasks = new List<ApiTask>();
            _dbContextMock.Setup(c => c.Tasks).ReturnsDbSet(tasks);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var request = new CreateTaskRequest { Title = "New Task", ProjectId = 1 };
            var newId = await _taskService.AddTaskAsync(request);
            _dbContextMock.Verify(m => m.Tasks.Add(It.IsAny<ApiTask>()), Times.Once());
            Assert.True(newId >= 0);
        }

        [Fact]
        // Tests that a task's properties are correctly updated.
        public async Task UpdateTaskAsync_ReturnsTrue_WhenUpdated()
        {
            var taskInDb = new ApiTask { Id = 1, Title = "Old Title" };
            _dbContextMock.Setup(c => c.Tasks.FindAsync(1)).ReturnsAsync(taskInDb);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var request = new UpdateTaskRequest { Id = 1, Title = "New Title", ProjectId = 1 };
            var result = await _taskService.UpdateTaskAsync(request);
            Assert.True(result);
            Assert.Equal("New Title", taskInDb.Title);
        }

        [Fact]
        // Tests that update fails if the task does not exist.
        public async Task UpdateTaskAsync_ReturnsFalse_WhenTaskDoesNotExist()
        {
            _dbContextMock.Setup(c => c.Tasks.FindAsync(99)).ReturnsAsync((ApiTask?)null);
            var request = new UpdateTaskRequest { Id = 99, Title = "Non-existent" };
            var result = await _taskService.UpdateTaskAsync(request);
            Assert.False(result);
        }

        [Fact]
        // Tests successful deletion of an existing task.
        public async Task DeleteTaskAsync_ReturnsTrue_WhenTaskExists()
        {
            var task = new ApiTask { Id = 1 };
            _dbContextMock.Setup(c => c.Tasks.FindAsync(1)).ReturnsAsync(task);
            _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var result = await _taskService.DeleteTaskAsync(1);
            Assert.True(result);
            _dbContextMock.Verify(m => m.Tasks.Remove(task), Times.Once());
        }

        [Fact]
        // Tests that deletion fails if the task does not exist.
        public async Task DeleteTaskAsync_ReturnsFalse_WhenTaskDoesNotExist()
        {
            _dbContextMock.Setup(c => c.Tasks.FindAsync(99)).ReturnsAsync((ApiTask?)null);
            var result = await _taskService.DeleteTaskAsync(99);
            Assert.False(result);
        }
    }
}