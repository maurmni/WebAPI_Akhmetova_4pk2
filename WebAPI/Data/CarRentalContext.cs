using WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация связей
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CarId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Renter)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.RenterId);

            // Индексы для оптимизации
            modelBuilder.Entity<Car>()
                .HasIndex(c => c.IsAvailable);

            modelBuilder.Entity<Renter>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}