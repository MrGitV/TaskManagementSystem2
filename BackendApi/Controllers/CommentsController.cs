using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    // Controller to manage comments on tasks.
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        // GET api/comments/{taskId}
        [HttpGet("{taskId}")]
        [Authorize]
        public async Task<IActionResult> GetByTask(int taskId)
        {
            var comments = await _commentService.GetCommentsForTaskAsync(taskId);
            return Ok(comments);
        }

        // POST api/comments
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            // Get user ID from the token claims.
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            // Add the comment using the service.
            var success = await _commentService.AddCommentAsync(request, userId);
            if (!success) return StatusCode(500, "Failed to add comment");
            return CreatedAtAction(nameof(GetByTask), new { taskId = request.TaskId }, request);
        }
    }
}