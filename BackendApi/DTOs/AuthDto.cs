using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs
{
    // Request for login.
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }

    // Response after successful authentication.
    public class LoginResponse
    {
        public string? Token { get; set; }
        public int UserId { get; set; }
        public string? Role { get; set; }
    }
}