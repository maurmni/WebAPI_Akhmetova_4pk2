using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.DTOs;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IAuthService authService, ILogger<UsersController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// получить профиль тек пользователя
        /// </summary>
        /// <response code="200">профиль получен</response>
        /// <response code="401">не авторизован</response>
        [HttpGet("profile")]
        [Authorize]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<UserProfileDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 401)]
        public async Task<ActionResult<APIResponse<UserProfileDTO>>> GetProfile()
        {
            var nameIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nameIdClaim) || !int.TryParse(nameIdClaim, out var userId))
            {
                return Unauthorized(new APIResponse("Не авторизован"));
            }
            _logger.LogInformation("Получение профиля пользователя с ID {UserId}", userId);

            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new APIResponse("Пользователь не найден"));
            }

            var profile = new UserProfileDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(new APIResponse<UserProfileDTO>(profile, "Профиль успешно получен"));
        }

        /// <summary>
        /// получить список всех пользователей (только для админа)
        /// </summary>
        /// <response code="200">список пользователей успешно получен</response>
        /// <response code="403">недостаточно прав для получения</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<UserProfileDTO>>), 200)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<IEnumerable<UserProfileDTO>>>> GetAllUsers()
        {
            _logger.LogInformation("Получение списка всех пользователей");

            var users = await _authService.GetAllUsersAsync();
            var userProfiles = users.Select(u => new UserProfileDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();

            return Ok(new APIResponse<IEnumerable<UserProfileDTO>>(userProfiles, "Список пользователей успешно получен"));
        }

        /// <summary>
        /// обновить роль пользователя (только для админа)
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="updateRoleDto">данные для обновления роли</param>
        /// <response code="200">роль успешно обновлена</response>
        /// <response code="403">недостаточно прав</response>
        /// <response code="404">пользователь не найден</response>
        [HttpPut("{userId}/role")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(APIResponse<UserProfileDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse<UserProfileDTO>>> UpdateUserRole(int userId, [FromBody] UpdateRoleDTO updateRoleDto)
        {
            _logger.LogInformation("Обновление роли пользователя с ID {UserId} на {Role}", userId, updateRoleDto.Role);

            var updatedUser = await _authService.UpdateUserRoleAsync(userId, updateRoleDto.Role);
            if (updatedUser == null)
            {
                return NotFound(new APIResponse("Пользователь не найден"));
            }

            var profile = new UserProfileDTO
            {
                Id = updatedUser.Id,
                Username = updatedUser.Username,
                Email = updatedUser.Email,
                Role = updatedUser.Role,
                CreatedAt = updatedUser.CreatedAt
            };

            return Ok(new APIResponse<UserProfileDTO>(profile, "Роль пользователя успешно обновлена"));
        }
    }

    /// <summary>
    /// DTO для профиля пользователя
    /// </summary>
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO для обновления роли пользователя
    /// </summary>
    public class UpdateRoleDTO
    {
        public string Role { get; set; } = null!;
    }
}
