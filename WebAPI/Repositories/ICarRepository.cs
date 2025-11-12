using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand);
        Task<IEnumerable<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<Car?> GetCarWithRentalsAsync(int id);
        Task<bool> IsLicensePlateUniqueAsync(string carPlate, int? excludeId = null);
    }
}
