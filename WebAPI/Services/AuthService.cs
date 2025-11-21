using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;
using WebAPI.Models.DTO;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    /// <summary>
    /// сервис для аутентификации и управления пользователями
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly PasswordHasher<User> _passwordHasher;

        /// <summary>
        /// конструктор сервиса аутентификации
        /// </summary>
        public AuthService(
            IRepository<User> userRepository,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        /// <summary>
        /// регистрация нового пользователя
        /// </summary>
        public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO registerDto)
        {
            if (await UserExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException($"Пользователь с email {registerDto.Email} уже существует");
            }

            if (await UsernameExistsAsync(registerDto.Username))
            {
                throw new InvalidOperationException($"Пользователь с именем {registerDto.Username} уже существует");
            }

            var role = registerDto.Role ?? "User";
            if (role != "User" && role != "Admin" && role != "Manager")
            {
                throw new InvalidOperationException("Недопустимая роль пользователя");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            var createdUser = await _userRepository.AddAsync(user);
            _logger.LogInformation("Зарегистрирован новый пользователь: {Username} с ролью {Role}", user.Username, user.Role);

            return new RegisterResponseDTO
            {
                UserId = (createdUser.Id).ToString(),
                Username = createdUser.Username,
                Email = createdUser.Email,
                Message = "Пользователь успешно зарегистрирован"
            };
        }

        /// <summary>
        /// аутентификация пользователя
        /// </summary>
        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto)
        {
            var user = await GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Неверный email или пароль");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Неверный email или пароль");
            }

            var token = GenerateJwtToken(user);
            _logger.LogInformation("Пользователь {Username} с ролью {Role} успешно вошел в систему", user.Username, user.Role);

            var expireHours = 24;
            if (int.TryParse(_configuration.GetSection("Jwt")["ExpireHours"], out var eh))
            {
                expireHours = eh;
            }

            return new AuthResponseDTO
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token,
                Expires = DateTime.UtcNow.AddHours(expireHours),
                Message = "Успешный вход в систему"
            };
        }

        /// <summary>
        /// получить пользователя по ID
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// получить всех пользователей (только для админа)
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        /// <summary>
        /// обновить роль пользователя (только для админа)
        /// </summary>
        public async Task<User?> UpdateUserRoleAsync(int userId, string newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            if (newRole != "User" && newRole != "Admin" && newRole != "Manager")
            {
                throw new InvalidOperationException("Недопустимая роль пользователя");
            }

            user.Role = newRole;
            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Роль пользователя {Username} изменена на {Role}", user.Username, user.Role);

            return updatedUser;
        }

        /// <summary>
        /// проверить существование пользователя по почте
        /// </summary>
        public async Task<bool> UserExistsAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            return users.Any();
        }

        /// <summary>
        /// проверить существование пользователя по юзернейму
        /// </summary>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            var users = await _userRepository.FindAsync(u => u.Username == username);
            return users.Any();
        }

        /// <summary>
        /// генерация JWT токен для пользователя
        /// </summary>
        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var expireHours = 24;
            if (int.TryParse(jwtSettings["ExpireHours"], out var eh))
            {
                expireHours = eh;
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// получить пользователя по почте
        /// </summary>
        private async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            return users.FirstOrDefault();
        }
    }
}
