using WebAPI.Models;

namespace WebAPI.Data
{
    public static class Data
    {
        public static void Initialize(CarRentalContext context)
        {
            if (context.Cars.Any() || context.Renters.Any() || context.Rentals.Any())
            {
                return; 
            }
        }
    }
}