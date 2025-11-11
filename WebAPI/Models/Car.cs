using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Car
    {
        [Required]
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public bool IsAvailable { get; set; }
        [Required]
        public string CarPlate { get; set; } = null!;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
