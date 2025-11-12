using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetRentalsByCustomerIdAsync(int customerId);
        Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId);
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetCompletedRentalsAsync();
        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();
        Task<Rental?> GetRentalWithDetailsAsync(int id);
        Task<bool> IsCarAvailableForPeriodAsync(int carId, DateTime startDate, DateTime endDate, int? excludeRentalId = null);
    }
}
