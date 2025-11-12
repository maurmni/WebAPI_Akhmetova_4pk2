using static WebAPI.Models.DTO.RentalDTO;

namespace WebAPI.Services
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalResponseDTO>> GetAllRentalsAsync();
        Task<RentalResponseDTO?> GetRentalByIdAsync(int id);
        Task<RentalResponseDTO> CreateRentalAsync(RentalCreateDTO rentalDto);
        Task<RentalResponseDTO?> CompleteRentalAsync(int id, RentalCompleteDTO completeDto);
        Task<IEnumerable<RentalResponseDTO>> GetActiveRentalsAsync();
        Task<IEnumerable<RentalResponseDTO>> GetRentalsByRenterAsync(int renterId);
        Task<IEnumerable<RentalResponseDTO>> GetOverdueRentalsAsync();
    }
}