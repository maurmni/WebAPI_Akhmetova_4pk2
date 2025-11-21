using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public int RenterId { get; set; }
        public Renter Renter { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;  

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 

        public DateTime? ActualReturnDate { get; set; } 
        public decimal Price { get; set; }
        public string Status { get; set; } = "Active"; 
    }
}

