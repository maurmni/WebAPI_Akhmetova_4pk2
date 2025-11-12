using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services;
using static WebAPI.Models.DTO.RenterDTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentersController : ControllerBase
    {
        private readonly IRenterService _renterService;
        private readonly ILogger<RentersController> _logger;

        public RentersController(IRenterService renterService, ILogger<RentersController> logger)
        {
            _renterService = renterService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RenterResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<RenterResponseDTO>>>> GetRenters()
        {
            _logger.LogInformation("Получение всех арендаторов");
            var renters = await _renterService.GetAllRentersAsync();
            return Ok(new APIResponse<IEnumerable<RenterResponseDTO>>(renters, "Арендаторы успешно получены"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<RenterResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse  <RenterResponseDTO>>> GetRenter(int id)
        {
            _logger.LogInformation("Получение арендатора с ID {RenterId}", id);
            var renter = await _renterService.GetRenterByIdAsync(id);

            if (renter == null)
            {
                _logger.LogWarning("Арендатор с ID {RenterId} не найден", id);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse<RenterResponseDTO>(renter, "Арендатор успешно получен"));
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(APIResponse<RenterResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse<RenterResponseDTO>>> GetRenterByEmail(string email)
        {
            _logger.LogInformation("Получение арендатора с email {Email}", email);
            var renter = await _renterService.GetRenterByEmailAsync(email);

            if (renter == null)
            {
                _logger.LogWarning("Арендатор с email {Email} не найден", email);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse<RenterResponseDTO>(renter, "Арендатор успешно получен"));
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<RenterResponseDTO>), 201)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        public async Task<ActionResult<APIResponse<RenterResponseDTO>>> CreateRenter(RenterCreateDTO renterDto)
        {
            _logger.LogInformation("Создание нового арендатора");
            var renter = await _renterService.CreateRenterAsync(renterDto);

            return CreatedAtAction(
                nameof(GetRenter),
                new { id = renter.Id },
                new APIResponse<RenterResponseDTO>(renter, "Арендатор успешно создан")
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<RenterResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        public async Task<ActionResult<APIResponse<RenterResponseDTO>>> UpdateRenter(int id, RenterUpdateDTO renterDto)
        {
            _logger.LogInformation("Обновление арендатора с ID {RenterId}", id);
            var renter = await _renterService.UpdateRenterAsync(id, renterDto);

            if (renter == null)
            {
                _logger.LogWarning("Арендатор с ID {RenterId} не найден для обновления", id);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse<RenterResponseDTO>(renter, "Арендатор успешно обновлен"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIResponse), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse>> DeleteRenter(int id)
        {
            _logger.LogInformation("Удаление арендатора с ID {RenterId}", id);
            var result = await _renterService.DeleteRenterAsync(id);

            if (!result)
            {
                _logger.LogWarning("Арендатор с ID {RenterId} не найден для удаления", id);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse(true, "Арендатор успешно удален"));
        }
    }
}