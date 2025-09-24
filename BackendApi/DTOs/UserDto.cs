using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs
{
    // Request to create a new user.
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [MaxLength(200)]
        public string? FullName { get; set; }
    }

    // Response for user data.
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public int TaskCount { get; set; }
    }
}