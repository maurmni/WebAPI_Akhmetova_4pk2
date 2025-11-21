using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Renter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        public string DriverLicenseNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
