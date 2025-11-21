using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        public bool IsAvailable { get; set; }

        [Required]
        public string CarPlate { get; set; } = null!;

        public decimal DailyPrice { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
