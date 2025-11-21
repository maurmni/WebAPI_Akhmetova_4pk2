using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.DTO;
using WebAPI.DTOs;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <param name="registerDto">данные для регистрации</param>
        /// <response code="201">пользователь успешно зарегистрирован</response>
        /// <response code="400">данные или пользователь уже существует</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponseDTO), 201)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        public async Task<ActionResult<APIResponse<RegisterResponseDTO>>> Register([FromBody] RegisterRequestDTO registerDto)
        {
            _logger.LogInformation("Запрос на регистрацию пользователя: {Email}", registerDto.Email);

            var result = await _authService.RegisterAsync(registerDto);

            return CreatedAtAction(nameof(Register),
                new { id = result.UserId },
                new APIResponse<RegisterResponseDTO>(result, "Пользователь успешно зарегистрирован"));
        }

        /// <param name="loginDto">данные для входа</param>
        /// <response code="200">успешный вход</response>
        /// <response code="401">неверные учетные данные</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(APIResponse<AuthResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 401)]
        public async Task<ActionResult<APIResponse<AuthResponseDTO>>> Login([FromBody] LoginRequestDTO loginDto)
        {
            _logger.LogInformation("Запрос на вход пользователя: {Email}", loginDto.Email);

            var result = await _authService.LoginAsync(loginDto);

            return Ok(new APIResponse<AuthResponseDTO>(result, "Успешный вход в систему"));
        }

        /// <param name="email">email для проверки</param>
        /// <response code="200">результат проверки</response>
        [HttpGet("check-email/{email}")]
        [ProducesResponseType(typeof(APIResponse<bool>), 200)]
        public async Task<ActionResult<APIResponse<bool>>> CheckEmailExists(string email)
        {
            var exists = await _authService.UserExistsAsync(email);
            return Ok(new APIResponse<bool>(exists, exists ? "Email уже занят" : "Email доступен"));
        }

        /// <param name="username">имя пользователя для проверки</param>
        /// <response code="200">результат проверки</response>
        [HttpGet("check-username/{username}")]
        [ProducesResponseType(typeof(APIResponse<bool>), 200)]
        public async Task<ActionResult<APIResponse<bool>>> CheckUsernameExists(string username)
        {
            var exists = await _authService.UsernameExistsAsync(username);
            return Ok(new APIResponse<bool>(exists, exists ? "Имя пользователя уже занято" : "Имя пользователя доступно"));
        }
    }
}
