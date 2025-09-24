namespace ClientApp.DTOs
{
    // Represents the response received after a successful login.
    public class LoginResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}