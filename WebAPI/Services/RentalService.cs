using AutoMapper;
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories;
using static WebAPI.Models.DTO.RentalDTO;

namespace WebAPI.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ICarRepository _carRepository;
        private readonly IRenterRepository _renterRepository;
        private readonly IMapper _mapper;

        public RentalService(
            IRentalRepository rentalRepository,
            ICarRepository carRepository,
            IRenterRepository renterRepository,
            IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _carRepository = carRepository;
            _renterRepository = renterRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetAllRentalsAsync()
        {
            var rentals = await _rentalRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RentalResponseDTO>>(rentals);
        }

        public async Task<RentalResponseDTO?> GetRentalByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsAsync(id);
            return _mapper.Map<RentalResponseDTO?>(rental);
        }

        public async Task<RentalResponseDTO> CreateRentalAsync(RentalCreateDTO rentalDto)
        {
            var car = await _carRepository.GetByIdAsync(rentalDto.CarId);
            if (car == null)
                throw new ArgumentException($"Автомобиль с ID {rentalDto.CarId} не найден");

            var renter = await _renterRepository.GetByIdAsync(rentalDto.RenterId);
            if (renter == null)
                throw new ArgumentException($"Арендатор с ID {rentalDto.RenterId} не найден");

            if (!car.IsAvailable)
                throw new InvalidOperationException($"Автомобиль {car.Brand} {car.Model} недоступен для аренды");

            var isAvailable = await _rentalRepository.IsCarAvailableForPeriodAsync(
                rentalDto.CarId, rentalDto.StartDate, rentalDto.EndDate);

            if (!isAvailable)
                throw new InvalidOperationException($"Автомобиль занят на указанный период");

            var rentalDays = (rentalDto.EndDate - rentalDto.StartDate).Days;
            var totalPrice = car.DailyPrice * rentalDays;

            var rental = _mapper.Map<Rental>(rentalDto);

            car.IsAvailable = false;
            await _carRepository.UpdateAsync(car);

            var createdRental = await _rentalRepository.AddAsync(rental);
            var rentalWithDetails = await _rentalRepository.GetRentalWithDetailsAsync(createdRental.Id);

            return _mapper.Map<RentalResponseDTO>(rentalWithDetails);
        }
        public async Task<RentalResponseDTO?> CompleteRentalAsync(int id, RentalCompleteDTO completeDto)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsAsync(id);
            if (rental == null)
                return null;

            rental.ReturnDate = completeDto.ActualReturnDate;

            var car = await _carRepository.GetByIdAsync(rental.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
                await _carRepository.UpdateAsync(car);
            }

            var updatedRental = await _rentalRepository.UpdateAsync(rental);
            return _mapper.Map<RentalResponseDTO>(updatedRental);
        }
        public async Task<IEnumerable<RentalResponseDTO>> GetActiveRentalsAsync()
        {
            var rentals = await _rentalRepository.GetActiveRentalsAsync();
            return _mapper.Map<IEnumerable<RentalResponseDTO>>(rentals);
        }
        public async Task<IEnumerable<RentalResponseDTO>> GetRentalsByRenterAsync(int renterId)
        {
            var rentals = await _rentalRepository.GetRentalsByCarIdAsync(renterId);
            return _mapper.Map<IEnumerable<RentalResponseDTO>>(rentals);
        }
        public async Task<IEnumerable<RentalResponseDTO>> GetOverdueRentalsAsync()
        {
            var rentals = await _rentalRepository.GetOverdueRentalsAsync();
            return _mapper.Map<IEnumerable<RentalResponseDTO>>(rentals);
        }
    }
}