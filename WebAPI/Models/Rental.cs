namespace WebAPI.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int RenterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Price { get; set; }

        public Car Car { get; set; } = null!;
        public Renter Renter { get; set; } = null!;
    }
}
