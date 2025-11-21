using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.DTOs;
using WebAPI.Services;
using static WebAPI.Models.DTO.RentalDTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(IRentalService rentalService, ILogger<RentalsController> logger)
        {
            _rentalService = rentalService;
            _logger = logger;
        }

        /// <summary>
        /// получить все аренды (только для админа и менеджера)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RentalResponseDTO>>), 200)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<IEnumerable<RentalResponseDTO>>>> GetRentals()
        {
            _logger.LogInformation("Получение всех аренд");
            var rentals = await _rentalService.GetAllRentalsAsync();
            return Ok(new APIResponse<IEnumerable<RentalResponseDTO>>(rentals, "Аренды успешно получены"));
        }

        /// <summary>
        /// получить мои аренды (для тек пользователя)
        /// </summary>
        [HttpGet("my-rentals")]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RentalResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<RentalResponseDTO>>>> GetMyRentals()
        {
            var nameIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nameIdClaim) || !int.TryParse(nameIdClaim, out var userId))
            {
                return Unauthorized(new APIResponse("Не авторизован"));
            }

            _logger.LogInformation("Получение аренд пользователя с ID {UserId}", userId);

            var rentals = await _rentalService.GetRentalsByUserAsync(userId);
            return Ok(new APIResponse<IEnumerable<RentalResponseDTO>>(rentals, "Aренды успешно получены"));
        }

        /// <summary>
        /// аолучить аренду по ID (доступно создателю аренды, админу и менеджеру)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<RentalResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse<RentalResponseDTO>>> GetRental(int id)
        {
            _logger.LogInformation("Получение аренды с ID {RentalId}", id);
            var rental = await _rentalService.GetRentalByIdAsync(id);

            if (rental == null)
            {
                return NotFound(new APIResponse("Аренда не найдена"));
            }

            // проверка доступа
            var nameIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(nameIdClaim ?? "0", out var userId))
            {
                return Unauthorized(new APIResponse("Не авторизован"));
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Admin" && userRole != "Manager" && rental.UserId != userId)
            {
                return Forbid();
            }

            return Ok(new APIResponse<RentalResponseDTO>(rental, "Аренда успешно получена"));
        }

        /// <summary>
        /// создать новую аренду (доступно всем пользователям)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<RentalResponseDTO>), 201)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        public async Task<ActionResult<APIResponse<RentalResponseDTO>>> CreateRental([FromBody] RentalCreateDTO rentalDto)
        {
            _logger.LogInformation("Создание новой аренды");

            var nameIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nameIdClaim) || !int.TryParse(nameIdClaim, out var userId))
            {
                return Unauthorized(new APIResponse("Не авторизован"));
            }

            var rental = await _rentalService.CreateRentalAsync(rentalDto, userId);

            return CreatedAtAction(
                nameof(GetRental),
                new { id = rental.Id },
                new APIResponse<RentalResponseDTO>(rental, "Аренда успешно создана")
            );
        }

        /// <summary>
        /// завершить аренду (только для админа и менеджера)
        /// </summary>
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<RentalResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<RentalResponseDTO>>> CompleteRental(int id, [FromBody] RentalCompleteDTO completeDto)
        {
            _logger.LogInformation("Завершение аренды с ID {RentalId}", id);
            var rental = await _rentalService.CompleteRentalAsync(id, completeDto);

            if (rental == null)
            {
                return NotFound(new APIResponse("Аренда не найдена"));
            }

            return Ok(new APIResponse<RentalResponseDTO>(rental, "Аренда успешно завершена"));
        }

        /// <summary>
        /// получить активные аренды (только для админа и менеджера)
        /// </summary>
        [HttpGet("active")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RentalResponseDTO>>), 200)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<IEnumerable<RentalResponseDTO>>>> GetActiveRentals()
        {
            _logger.LogInformation("Получение активных аренд");
            var rentals = await _rentalService.GetActiveRentalsAsync();
            return Ok(new APIResponse<IEnumerable<RentalResponseDTO>>(rentals, "Активные аренды успешно получены"));
        }

        /// <summary>
        /// получить просроченные аренды (только для админа и менеджера)
        /// </summary>
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RentalResponseDTO>>), 200)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<IEnumerable<RentalResponseDTO>>>> GetOverdueRentals()
        {
            _logger.LogInformation("Получение просроченных аренд");
            var rentals = await _rentalService.GetOverdueRentalsAsync();
            return Ok(new APIResponse<IEnumerable<RentalResponseDTO>>(rentals, "Просроченные аренды успешно получены"));
        }
    }
}
