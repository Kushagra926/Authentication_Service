namespace Authentication_Servie.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? PasswordHash { get; set; }
        public string Provider { get; set; }
        public string? ProviderId { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
