namespace WebAPI.Models.DTO
{
    public class RegisterResponseDTO
    {
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
