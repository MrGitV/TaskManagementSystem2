using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;
using System.Security.Claims;

namespace TaskManagementAPI.Controllers
{
    // Controller for managing tasks.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;

        // GET api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Only Admins should get all tasks
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var tasks = await _taskService.GetAllTasksAsync(startDate, endDate);
            return Ok(tasks);
        }

        // GET api/tasks/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksForUser(int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            if (User.IsInRole("Admin") || currentUserId == userId.ToString())
            {
                var tasks = await _taskService.GetTasksForUserAsync(userId, startDate, endDate);
                return Ok(tasks);
            }

            return Forbid();
        }

        // POST api/tasks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            var newId = await _taskService.AddTaskAsync(request);
            var newTask = await _taskService.GetTaskByIdAsync(newId);
            return CreatedAtAction(nameof(GetAll), null, new { id = newId });
        }

        // GET api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // PUT api/tasks/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            var success = await _taskService.UpdateTaskAsync(request);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE api/tasks/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _taskService.DeleteTaskAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}