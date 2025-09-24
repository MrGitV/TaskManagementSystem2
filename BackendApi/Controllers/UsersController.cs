using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    // Controller for managing users.
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        // GET api/users/exists/{username}
        [HttpGet("exists/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserExistsAsync(string username)
        {
            if (await _userService.UserExistsAsync(username))
            {
                return Ok(true);
            }
            return NotFound();
        }

        // GET api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // POST api/users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (request.Username != null && await _userService.UserExistsAsync(request.Username))
            {
                return Conflict("Username already exists");
            }

            var success = await _userService.CreateUserAsync(request);
            if (!success) return StatusCode(500, "Failed to create user");
            return CreatedAtAction(nameof(GetAll), null, request);
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}