using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(CarRentalContext context) : base(context) { }

        public async Task<IEnumerable<Rental>> GetRentalsByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(r => r.RenterId == customerId)
                .Include(r => r.Car)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId)
        {
            return await _dbSet
                .Where(r => r.CarId == carId)
                .Include(r => r.Renter)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.Renter)
                .OrderBy(r => r.ReturnDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetCompletedRentalsAsync()
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.RenterId)
                .OrderByDescending(r => r.ReturnDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            var currentDate = DateTime.Now;
            return await _dbSet
                .Where(r => r.ReturnDate < currentDate)
                .Include(r => r.Car)
                .Include(r => r.Renter)
                .OrderBy(r => r.ReturnDate)
                .ToListAsync();
        }

        public async Task<Rental?> GetRentalWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.Renter)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> IsCarAvailableForPeriodAsync(int carId, DateTime startDate, DateTime endDate, int? excludeRentalId = null)
        {
            var query = _dbSet.Where(r =>
                r.CarId == carId &&
                ((r.StartDate <= endDate && r.ReturnDate >= startDate)));

            if (excludeRentalId.HasValue)
            {
                query = query.Where(r => r.Id != excludeRentalId.Value);
            }

            return !await query.AnyAsync();
        }

        public override async Task<Rental?> GetByIdAsync(int id)
        {
            return await GetRentalWithDetailsAsync(id);
        }

        public override async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.Renter)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }
    }
}
