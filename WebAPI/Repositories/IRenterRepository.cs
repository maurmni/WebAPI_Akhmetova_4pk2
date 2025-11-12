using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IRenterRepository : IRepository<Renter>
    {
        Task<Renter?> GetByEmailAsync(string email);
        Task<Renter?> GetRenterWithRentalsAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<bool> IsDriverLicenseUniqueAsync(string driverLicense, int? excludeId = null);
    }
}