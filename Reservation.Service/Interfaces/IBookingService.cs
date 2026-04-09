using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;

namespace Reservation.Service.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto?> GetBookingByIdAsync(int id);
        Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto);
        Task<BookingDto?> UpdateBookingAsync(int id, CreateBookingDto updateBookingDto);
        Task<IEnumerable<BookingDto>> GetBookingsByGuestIdAsync(int guestId);
        Task<IEnumerable<BookingDto>> GetBookingsByStatusAsync(BookingStatus status);
        Task<bool> UpdateBookingStatusAsync(int id, BookingStatus status);
        Task<bool> CancelBookingAsync(int id);
        Task<bool> ConfirmBookingAsync(int id);
        Task<IEnumerable<BookingDto>> GetPendingBookingsForHostAsync(int hostId);
    }
}
