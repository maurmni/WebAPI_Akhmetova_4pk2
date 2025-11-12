namespace WebAPI.Models.DTO
{
    public class CarDTO
    {
        public class CarResponseDTO
        {
            public int Id { get; set; }
            public string Brand { get; set; } = null!;
            public string Model { get; set; } = null!;
            public bool IsAvailable { get; set; }
            public string CarPlate { get; set; } = null!;
            public decimal DailyPrice { get; set; }
        }
        public class CarCreateDTO
        {
            public string Brand { get; set; } = null!;
            public string Model { get; set; } = null!;
            public string CarPlate { get; set; } = null!;
            public decimal DailyPrice { get; set; }
        }
        public class CarUpdateDTO
        {
            public string Brand { get; set; } = null!;
            public string Model { get; set; } = null!;
            public string CarPlate { get; set; } = null!;
            public decimal DailyPrice { get; set; }
            public bool IsAvailable { get; set; }
        }
    }
}
