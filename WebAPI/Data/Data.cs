using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
namespace WebAPI.Data
{
    public class Data
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CarRentalContext(
                serviceProvider.GetRequiredService<DbContextOptions<CarRentalContext>>()))
            {
                if (context.Cars.Any() || context.Renters.Any() || context.Rentals.Any())
                {
                    return; // DB has been seeded
                }

                // Добавление автомобилей
                var cars = new Car[]
                {
                    new Car { Brand = "Toyota", Model = "Camry", CarPlate = "ABC123", IsAvailable = true },
                    new Car { Brand = "Honda", Model = "Civic", CarPlate = "DEF456", IsAvailable = true },
                    new Car { Brand = "BMW", Model = "X5", CarPlate = "GHI789", IsAvailable = true },
                    new Car { Brand = "Mercedes", Model = "C-Class", CarPlate = "JKL012", IsAvailable = true },
                    new Car { Brand = "Audi", Model = "A4", CarPlate = "MNO345", IsAvailable = true }
                };

                context.Cars.AddRange(cars);
                context.SaveChanges();

                // Добавление клиентов
                var customers = new Renter[]
                {
                    new Renter { FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", Phone = "+1234567890", DriverLicense = "DL123456" },
                    new Renter { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", Phone = "+0987654321", DriverLicense = "DL654321" },
                    new Renter { FirstName = "Mike", LastName = "Johnson", Email = "mike.johnson@email.com", Phone = "+1122334455", DriverLicense = "DL112233" },
                    new Renter { FirstName = "Sarah", LastName = "Wilson", Email = "sarah.wilson@email.com", Phone = "+5566778899", DriverLicense = "DL445566" },
                    new Renter { FirstName = "Tom", LastName = "Brown", Email = "tom.brown@email.com", Phone = "+9988776655", DriverLicense = "DL778899" }
                };

                context.Renters.AddRange(customers);
                context.SaveChanges();

                // Добавление аренд
                var rentals = new Rental[]
                {
                    new Rental { CarId = 1, RenterId = 1, StartDate = DateTime.Now.AddDays(-5), ReturnDate = DateTime.Now.AddDays(2), Price = 315.00m},
                    new Rental { CarId = 2, RenterId = 2, StartDate = DateTime.Now.AddDays(-10), ReturnDate = DateTime.Now.AddDays(-3), Price = 280.00m},
                    new Rental { CarId = 3, RenterId = 3, StartDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now.AddDays(0), Price = 560.00m},
                    new Rental { CarId = 4, RenterId = 4, StartDate = DateTime.Now.AddDays(-15), ReturnDate = DateTime.Now.AddDays(-8), Price = 525.00m},
                    new Rental { CarId = 5, RenterId = 5, StartDate = DateTime.Now.AddDays(-3), ReturnDate = DateTime.Now.AddDays(4), Price = 455.00m}
                };

                context.Rentals.AddRange(rentals);
                context.SaveChanges();
            }
        }
    }
}
