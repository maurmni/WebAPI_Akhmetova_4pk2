using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;

        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carRepository.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Car>>> GetAvailableCars()
        {
            var cars = await _carRepository.GetAvailableCarsAsync();
            return Ok(cars);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> CreateCar(Car car)
        {
            if (!await _carRepository.IsLicensePlateUniqueAsync(car.CarPlate))
            {
                ModelState.AddModelError("LicensePlate", "Car with this license plate already exists.");
                return BadRequest(ModelState);
            }

            var createdCar = await _carRepository.AddAsync(car);
            return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            if (!await _carRepository.IsLicensePlateUniqueAsync(car.CarPlate, id))
            {
                ModelState.AddModelError("LicensePlate", "Car with this license plate already exists.");
                return BadRequest(ModelState);
            }

            try
            {
                await _carRepository.UpdateAsync(car);
            }
            catch (Exception)
            {
                if (!await _carRepository.ExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var result = await _carRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}