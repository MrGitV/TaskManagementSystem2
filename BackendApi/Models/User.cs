using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    // Represents a user account.
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        [MaxLength(200)]
        public string? FullName { get; set; }

        public UserRole Role { get; set; }
        public virtual ICollection<Task> Tasks { get; set; } = [];
    }
}