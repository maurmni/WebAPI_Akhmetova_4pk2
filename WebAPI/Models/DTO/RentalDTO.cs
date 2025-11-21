namespace WebAPI.Models.DTO
{
    public class RentalDTO
    {
        public class RentalResponseDTO
        {
            public int Id { get; set; }
            public int CarId { get; set; }
            public int RenterId { get; set; }
            public int UserId { get; set; } 
            public string CarBrand { get; set; } = null!;
            public string CarModel { get; set; } = null!;
            public string RenterName { get; set; } = null!;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; } 
            public DateTime? ActualReturnDate { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class RentalCreateDTO
        {
            public int CarId { get; set; }
            public int RenterId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        public class RentalCompleteDTO
        {
            public DateTime ActualReturnDate { get; set; }
        }
    }
}
