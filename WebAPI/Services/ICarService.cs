using static WebAPI.Models.DTO.CarDTO;

namespace WebAPI.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarResponseDTO>> GetAllCarsAsync();
        Task<CarResponseDTO?> GetCarByIdAsync(int id);
        Task<IEnumerable<CarResponseDTO>> GetAvailableCarsAsync();
        Task<CarResponseDTO> CreateCarAsync(CarCreateDTO carDto);
        Task<CarResponseDTO?> UpdateCarAsync(int id, CarUpdateDTO carDto);
        Task<bool> DeleteCarAsync(int id);
        Task<IEnumerable<CarResponseDTO>> GetCarsByBrandAsync(string brand);
    }
}