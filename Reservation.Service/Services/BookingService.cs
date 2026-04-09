using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;

        public BookingService(IMapper mapper, ReservationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            return booking == null ? null : _mapper.Map<BookingDto>(booking);
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            var property = await _context.Properties.FindAsync(createBookingDto.PropertyId);
            if (property == null)
                throw new InvalidOperationException("Property not found.");

            var guest = await _context.Users.FindAsync(createBookingDto.GuestId);
            if (guest == null)
                throw new InvalidOperationException("Guest not found.");

            if (createBookingDto.StartDate >= createBookingDto.EndDate)
                throw new InvalidOperationException("Start date must be before end date.");

            if (createBookingDto.StartDate < DateTime.Today)
                throw new InvalidOperationException("Cannot book for past dates.");

            var conflictingBookings = await _context.Bookings
                .Where(b => b.PropertyId == createBookingDto.PropertyId &&
                            b.Status != BookingStatus.Cancelled &&
                            b.StartDate < createBookingDto.EndDate &&
                            b.EndDate > createBookingDto.StartDate)
                .AnyAsync();

            if (conflictingBookings)
                throw new InvalidOperationException("Sajnos ez az ingatlan már foglalt a kiválasztott időpontban.");

            var booking = _mapper.Map<Booking>(createBookingDto);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<BookingDto?> UpdateBookingAsync(int id, CreateBookingDto updateBookingDto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return null;

            if (booking.Status == BookingStatus.Confirmed)
                throw new InvalidOperationException("Cannot modify confirmed booking.");

            if (updateBookingDto.StartDate >= updateBookingDto.EndDate)
                throw new InvalidOperationException("Start date must be before end date.");

            _mapper.Map(updateBookingDto, booking);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByGuestIdAsync(int guestId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Property)
                .Where(b => b.GuestId == guestId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByPropertyIdAsync(int propertyId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.PropertyId == propertyId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }


        public async Task<bool> UpdateBookingStatusAsync(int id, BookingStatus status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            booking.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            if (booking.Status == BookingStatus.Cancelled)
                return false;

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            if (booking.Status != BookingStatus.Pending)
                return false;

            booking.Status = BookingStatus.Confirmed;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<BookingDto>> GetPendingBookingsForHostAsync(int hostId)
        {
            var pendingBookings = await _context.Bookings
                .Include(b => b.Property)
                .Where(b => b.Property.HostId == hostId && b.Status == BookingStatus.Pending)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BookingDto>>(pendingBookings);
        }
    }
}
