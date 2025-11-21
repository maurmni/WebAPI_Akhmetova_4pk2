using WebAPI.Models;
using WebAPI.Models.DTO;

namespace WebAPI.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO registerDto);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto);
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> UpdateUserRoleAsync(int userId, string newRole);
        Task<bool> UserExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        string GenerateJwtToken(User user);
    }
}
