using AutoMapper;
using WebAPI.Models;
using static WebAPI.Models.DTO.CarDTO;
using static WebAPI.Models.DTO.RentalDTO;
using static WebAPI.Models.DTO.RenterDTO;

namespace WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarResponseDTO>();
            CreateMap<CarCreateDTO, Car>();
            CreateMap<CarUpdateDTO, Car>();

            CreateMap<Renter, RenterResponseDTO>();
            CreateMap<RenterCreateDTO, Renter>();
            CreateMap<RenterUpdateDTO, Renter>();

            CreateMap<Rental, RentalResponseDTO>()
                .ForMember(dest => dest.CarBrand, opt => opt.MapFrom(src => src.Car.Brand))
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car.Model))
                .ForMember(dest => dest.RenterName, opt => opt.MapFrom(src => $"{src.Renter.FirstName} {src.Renter.LastName}"));

            CreateMap<RentalCreateDTO, Rental>();
        }
    }
}
