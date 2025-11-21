using Microsoft.AspNetCore.Identity;
using WebAPI.Models;

namespace WebAPI.Data
{
    public static class Data
    {
        public static void Initialize(CarRentalContext context)
        {
            if (!context.Users.Any())
            {
                var users = new User[]
                {
                    new User
                    {
                        Username = "admin",
                        Email = "admin@gmail.com",
                        PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "manager1",
                        Email = "manager1@gmail.com",
                        PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123"),
                        Role = "Manager",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "user1",
                        Email = "user1@gmail.com",
                        PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "user2",
                        Email = "user2@gmail.com",
                        PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
