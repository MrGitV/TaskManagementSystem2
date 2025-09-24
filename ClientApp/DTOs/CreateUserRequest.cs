namespace ClientApp.DTOs
{
    // Represents the request payload for creating a new user.
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}