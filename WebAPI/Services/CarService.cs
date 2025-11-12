using AutoMapper;
using WebAPI.Models;
using WebAPI.Repositories;
using static WebAPI.Models.DTO.CarDTO;

namespace WebAPI.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        public CarService(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CarResponseDTO>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CarResponseDTO>>(cars);
        }
        public async Task<CarResponseDTO?> GetCarByIdAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            return _mapper.Map<CarResponseDTO?>(car);
        }
        public async Task<IEnumerable<CarResponseDTO>> GetAvailableCarsAsync()
        {
            var cars = await _carRepository.GetAvailableCarsAsync();
            return _mapper.Map<IEnumerable<CarResponseDTO>>(cars);
        }
        public async Task<CarResponseDTO> CreateCarAsync(CarCreateDTO carDto)
        {
            if (!await _carRepository.IsLicensePlateUniqueAsync(carDto.CarPlate))
            {
                throw new InvalidOperationException($"Автомобиль с номерным знаком {carDto.CarPlate} уже существует");
            }

            var car = _mapper.Map<Car>(carDto);
            car.IsAvailable = true;

            var createdCar = await _carRepository.AddAsync(car);
            return _mapper.Map<CarResponseDTO>(createdCar);
        }
        public async Task<CarResponseDTO?> UpdateCarAsync(int id, CarUpdateDTO carDto)
        {
            var existingCar = await _carRepository.GetByIdAsync(id);
            if (existingCar == null)
                return null;

            if (!await _carRepository.IsLicensePlateUniqueAsync(carDto.CarPlate, id))
            {
                throw new InvalidOperationException($"Автомобиль с номерным знаком {carDto.CarPlate} уже существует");
            }

            _mapper.Map(carDto, existingCar);
            var updatedCar = await _carRepository.UpdateAsync(existingCar);
            return _mapper.Map<CarResponseDTO>(updatedCar);
        }
        public async Task<bool> DeleteCarAsync(int id)
        {
            return await _carRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<CarResponseDTO>> GetCarsByBrandAsync(string brand)
        {
            var cars = await _carRepository.GetCarsByBrandAsync(brand);
            return _mapper.Map<IEnumerable<CarResponseDTO>>(cars);
        }
    }
}