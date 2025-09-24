namespace TaskManagementAPI
{
    // Defines the settings for JWT generation and validation.
    public class JwtSettings
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}