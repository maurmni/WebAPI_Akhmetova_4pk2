using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services;
using static WebAPI.Models.DTO.CarDTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> GetCar(int id)
        {   
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {   
                return NotFound(new APIResponse<CarResponseDTO>("Автомобиль не найден"));
            }
            return Ok(new APIResponse<CarResponseDTO>(car));
        }

        [HttpGet("available")]
        public async Task<ActionResult<APIResponse<IEnumerable<CarResponseDTO>>>> GetAvailableCars()
        {
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(new APIResponse<IEnumerable<CarResponseDTO>>(cars));
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> CreateCar(CarCreateDTO carDto)
        {
            var car = await _carService.CreateCarAsync(carDto);
            return CreatedAtAction(nameof(GetCar), new { id = car.Id },
                new APIResponse<CarResponseDTO>(car));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<CarResponseDTO>>> UpdateCar(int id, CarUpdateDTO carDto)
        {
            var car = await _carService.UpdateCarAsync(id, carDto);
            if (car == null)
            {
                return NotFound(new APIResponse<CarResponseDTO>("Автомобиль не найден"));
            }
            return Ok(new APIResponse<CarResponseDTO>(car));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> DeleteCar(int id)
        {
            var result = await _carService.DeleteCarAsync(id);
            if (!result)
            {
                return NotFound(new APIResponse<bool>("Автомобиль не найден"));
            }
            return Ok(new APIResponse<bool>(true, "Автомобиль удален"));
        }
    }
}