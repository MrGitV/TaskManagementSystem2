using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    // Controller to handle user authentication.
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Check if username or password are provided.
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Authenticate the user.
            var response = await _authService.AuthenticateAsync(request.Username, request.Password);

            // If authentication fails, return Unauthorized.
            if (response == null)
            {
                return Unauthorized();
            }

            // Return the successful login response.
            return Ok(response);
        }
    }
}
