using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Renter
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string DriverLicense { get; set; } = null!;
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
