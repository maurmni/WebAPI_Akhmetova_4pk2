using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class RenterRepository : Repository<Renter>, IRenterRepository
    {
        public RenterRepository(CarRentalContext context) : base(context) { }

        public async Task<Renter?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Renter?> GetRenterWithRentalsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Rentals)
                .ThenInclude(r => r.Car)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(c => c.Email.ToLower() == email.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<bool> IsDriverLicenseUniqueAsync(string driverLicense, int? excludeId = null)
        {
            var query = _dbSet.Where(c => c.DriverLicenseNumber.ToLower() == driverLicense.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public override async Task<IEnumerable<Renter>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }
    }
}
