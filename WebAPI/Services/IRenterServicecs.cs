using static WebAPI.Models.DTO.RenterDTO;

namespace WebAPI.Services
{
    public interface IRenterService
    {
        Task<IEnumerable<RenterResponseDTO>> GetAllRentersAsync();
        Task<RenterResponseDTO?> GetRenterByIdAsync(int id);
        Task<RenterResponseDTO?> GetRenterByEmailAsync(string email);
        Task<RenterResponseDTO> CreateRenterAsync(RenterCreateDTO renterDto);
        Task<RenterResponseDTO?> UpdateRenterAsync(int id, RenterUpdateDTO renterDto);
        Task<bool> DeleteRenterAsync(int id);
        Task<RenterResponseDTO?> GetRenterWithRentalsAsync(int id);
    }
}