using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services;
using static WebAPI.Models.DTO.CarDTO;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly ILogger<CarsController> _logger;

        public CarsController(ICarService carService, ILogger<CarsController> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        /// <summary>
        /// получить все автомобили (доступно всем пользователям)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CarResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetCars()
        {
            _logger.LogInformation("Получение всех автомобилей");
            var cars = await _carService.GetAllCarsAsync();
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars, "Автомобили успешно получены"));
        }

        /// <summary>
        /// получить автомобиль по ID (доступно всем пользователям)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<CarResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> GetCar(int id)
        {
            _logger.LogInformation("Получение автомобиля с ID {CarId}", id);
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
            {
                return NotFound(new APIResponse("Автомобиль не найден"));
            }

            return Ok(new APIResponse<CarResponseDTO>(car, "Автомобиль успешно получен"));
        }

        /// <summary>
        /// получить доступные автомобили (доступно всем пользователям)
        /// </summary>
        [HttpGet("available")]
        [Authorize(Roles = "User,Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CarResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetAvailableCars()
        {
            _logger.LogInformation("Получение доступных автомобилей");
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars, "Доступные автомобили успешно получены"));
        }

        /// <summary>
        /// создать новый автомобиль (только для админа и менеджера)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<CarResponseDTO>), 201)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> CreateCar([FromBody] CarCreateDTO carDto)
        {
            _logger.LogInformation("Создание нового автомобиля");
            var car = await _carService.CreateCarAsync(carDto);

            return CreatedAtAction(nameof(GetCar),
                new { id = car.Id },
                new APIResponse<CarResponseDTO>(car, "Автомобиль успешно создан")
            );
        }

        /// <summary>
        /// обновить данные автомобиля (только для админа и менеджера)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(APIResponse<CarResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> UpdateCar(int id, [FromBody] CarUpdateDTO carDto)
        {
            _logger.LogInformation("Обновление автомобиля с ID {CarId}", id);
            var car = await _carService.UpdateCarAsync(id, carDto);

            if (car == null)
            {
                return NotFound(new APIResponse("Автомобиль не найден"));
            }

            return Ok(new APIResponse<CarResponseDTO>(car, "Автомобиль успешно обновлен"));
        }

        /// <summary>
        /// удалить автомобиль (только для админа)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(APIResponse), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        [ProducesResponseType(typeof(APIResponse), 403)]
        public async Task<ActionResult<APIResponse>> DeleteCar(int id)
        {
            _logger.LogInformation("Удаление автомобиля с ID {CarId}", id);
            var result = await _carService.DeleteCarAsync(id);

            if (!result)
            {
                return NotFound(new APIResponse("Автомобиль не найден"));
            }

            return Ok(new APIResponse(true, "Автомобиль успешно удален"));
        }
    }
}
