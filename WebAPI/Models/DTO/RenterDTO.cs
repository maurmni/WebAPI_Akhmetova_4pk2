namespace WebAPI.Models.DTO
{
    public class RenterDTO
    {
        public class RenterResponseDTO
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public DateTime RegistrationDate { get; set; }
        }
        public class RenterCreateDTO
        {
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string DriverLicenseNumber { get; set; } = null!;
        }
        public class RenterUpdateDTO
        {
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string DriverLicenseNumber { get; set; } = null!;
        }
    }
}
