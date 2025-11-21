using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Renter> Renters { get; set; } = null!;
        public DbSet<Rental> Rentals { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CarId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Renter)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.RenterId);

            // связь Rental с User
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.IsAvailable);

            modelBuilder.Entity<Renter>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Renter>()
                .HasIndex(c => c.DriverLicenseNumber)
                .IsUnique();

            // конфиг для User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // нач данные администратора
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@gmail.com",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                });
        }

    }
}
