namespace ClientApp.DTOs
{
    // Represents a user data transfer object.
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int TaskCount { get; set; }
    }
}