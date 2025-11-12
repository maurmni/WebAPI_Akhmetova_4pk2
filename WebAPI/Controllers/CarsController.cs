using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using static WebAPI.Models.DTO.CarDTO;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CarResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetCars()
        {
            _logger.LogInformation("Получение всех автомобилей");
            var cars = await _carService.GetAllCarsAsync();
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars, "Автомобили успешно получены"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<CarResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> GetCar(int id)
        {
            _logger.LogInformation("Получение автомобиля с ID {CarId}", id);
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
            {
                _logger.LogWarning("Автомобиль с ID {CarId} не найден", id);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse<CarResponseDTO>(car, "Автомобиль успешно получен"));
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CarResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetAvailableCars()
        {
            _logger.LogInformation("Получение доступных автомобилей");
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars, "Доступные автомобили успешно получены"));
        }

        [HttpGet("brand/{brand}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CarResponseDTO>>), 200)]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetCarsByBrand(string brand)
        {
            _logger.LogInformation("Получение автомобилей бренда {Brand}", brand);
            var cars = await _carService.GetCarsByBrandAsync(brand);
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars, $"Автомобили бренда {brand} успешно получены"));
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CarResponseDTO>), 201)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> CreateCar(CarCreateDTO carDto)
        {
            _logger.LogInformation("Создание нового автомобиля");
            var car = await _carService.CreateCarAsync(carDto);

            return CreatedAtAction(
                nameof(GetCar),
                new { id = car.Id },
                new APIResponse<CarResponseDTO>(car, "Автомобиль успешно создан")
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<CarResponseDTO>), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        [ProducesResponseType(typeof(APIResponse), 400)]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> UpdateCar(int id, CarUpdateDTO carDto)
        {
            _logger.LogInformation("Обновление автомобиля с ID {CarId}", id);
            var car = await _carService.UpdateCarAsync(id, carDto);

            if (car == null)
            {
                _logger.LogWarning("Автомобиль с ID {CarId} не найден для обновления", id);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse<CarResponseDTO>(car, "Автомобиль успешно обновлен"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIResponse), 200)]
        [ProducesResponseType(typeof(APIResponse), 404)]
        public async Task<ActionResult<APIResponse>> DeleteCar(int id)
        {
            _logger.LogInformation("Удаление автомобиля с ID {CarId}", id);
            var result = await _carService.DeleteCarAsync(id);

            if (!result)
            {
                _logger.LogWarning("Автомобиль с ID {CarId} не найден для удаления", id);
                return NotFound(new APIResponse(false));
            }

            return Ok(new APIResponse(true, "Автомобиль успешно удален"));
        }
    }
}