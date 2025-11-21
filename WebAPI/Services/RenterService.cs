using AutoMapper;
using WebAPI.Models;
using WebAPI.Repositories;
using static WebAPI.Models.DTO.RentalDTO;
using static WebAPI.Models.DTO.RenterDTO;

namespace WebAPI.Services
{
    public class RenterService : IRenterService
    {
        private readonly IRenterRepository _renterRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RenterService(IRenterRepository renterRepository, IRentalRepository rentalRepository, IMapper mapper)
        {
            _renterRepository = renterRepository;
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RenterResponseDTO>> GetAllRentersAsync()
        {
            var renters = await _renterRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RenterResponseDTO>>(renters);
        }
        public async Task<RenterResponseDTO?> GetRenterByIdAsync(int id)
        {
            var renter = await _renterRepository.GetByIdAsync(id);
            return _mapper.Map<RenterResponseDTO?>(renter);
        }
        public async Task<RenterResponseDTO?> GetRenterByEmailAsync(string email)
        {
            var renter = await _renterRepository.GetByEmailAsync(email);
            return _mapper.Map<RenterResponseDTO?>(renter);
        }
        public async Task<RenterResponseDTO> CreateRenterAsync(RenterCreateDTO renterDto)
        {
            if (!await _renterRepository.IsEmailUniqueAsync(renterDto.Email))
            {
                throw new InvalidOperationException($"Арендатор с email {renterDto.Email} уже существует");
            }
            if (!await _renterRepository.IsDriverLicenseUniqueAsync(renterDto.DriverLicenseNumber))
            {
                throw new InvalidOperationException($"Арендатор с водительским удостоверением {renterDto.DriverLicenseNumber} уже существует");
            }

            var renter = _mapper.Map<Renter>(renterDto);
            var createdRenter = await _renterRepository.AddAsync(renter);
            return _mapper.Map<RenterResponseDTO>(createdRenter);
        }
        public async Task<RenterResponseDTO?> UpdateRenterAsync(int id, RenterUpdateDTO renterDto)
        {
            var existingRenter = await _renterRepository.GetByIdAsync(id);
            if (existingRenter == null)
                return null;

            if (!await _renterRepository.IsEmailUniqueAsync(renterDto.Email, id))
            {
                throw new InvalidOperationException($"Арендатор с email {renterDto.Email} уже существует");
            }
            if (!await _renterRepository.IsDriverLicenseUniqueAsync(renterDto.DriverLicenseNumber, id))
            {
                throw new InvalidOperationException($"Арендатор с водительским удостоверением {renterDto.DriverLicenseNumber} уже существует");
            }


            _mapper.Map(renterDto, existingRenter);
            var updatedRenter = await _renterRepository.UpdateAsync(existingRenter);
            return _mapper.Map<RenterResponseDTO>(updatedRenter);
        }
        public async Task<bool> DeleteRenterAsync(int id)
        {
            return await _renterRepository.DeleteAsync(id);
        }
        public async Task<RenterResponseDTO?> GetRenterWithRentalsAsync(int id)
        {
            var renter = await _renterRepository.GetRenterWithRentalsAsync(id);
            return _mapper.Map<RenterResponseDTO?>(renter);
        }
        /// <summary>
        /// получить аренды по ID пользователя
        /// </summary>
        public async Task<IEnumerable<RentalResponseDTO>> GetRentalsByUserAsync(int renterId)
        {
            var rentals = await _rentalRepository.FindAsync(r => r.RenterId == renterId);

            var list = new List<Rental>();
            foreach (var r in rentals)
            {
                var det = await _rentalRepository.GetRentalWithDetailsAsync(r.Id);
                if (det != null)
                    list.Add(det);
            }

            return _mapper.Map<IEnumerable<RentalResponseDTO>>(list);
        }

    }
}
