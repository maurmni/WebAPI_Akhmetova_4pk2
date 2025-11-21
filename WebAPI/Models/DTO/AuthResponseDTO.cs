namespace WebAPI.Models.DTO
{
    public class AuthResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string Message { get; set; } = null!;
    }
}
