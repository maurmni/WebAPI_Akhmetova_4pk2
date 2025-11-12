using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository 
    {
        public CarRepository(CarRentalContext context) : base(context) { }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _dbSet
                .Where(c => c.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand)
        {
            return await _dbSet
                .Where(c => c.Brand.ToLower() == brand.ToLower())
                .ToListAsync();
        }
        public async Task<IEnumerable<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Where(c => c.DailyPrice >= minPrice && c.DailyPrice <= maxPrice)
                .ToListAsync();
        }

        public async Task<Car?> GetCarWithRentalsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Rentals)
                .ThenInclude(r => r.Renter)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsLicensePlateUniqueAsync(string carPlate, int? excludeId = null)
        {
            var query = _dbSet.Where(c => c.CarPlate.ToLower() == carPlate.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public override async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Model)
                .ToListAsync();
        }
    }
}