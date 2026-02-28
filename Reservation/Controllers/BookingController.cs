using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("my-bookings")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var bookings = await _bookingService.GetBookingsByGuestIdAsync(userId);
            return Ok(bookings);
        }

       
        [Authorize(Roles = "Guest")]
        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var booking = await _bookingService.CreateBookingAsync(dto);
            return CreatedAtAction(nameof(GetMyBookings), new { id = booking.Id }, booking);
        }

        
        [Authorize(Roles = "Host,Admin")]
        [HttpPut("{id}/confirm")]
        public async Task<ActionResult> ConfirmBooking(int id)
        {
            var result = await _bookingService.ConfirmBookingAsync(id);
            return result ? Ok(new { message = "Confirmed" }) : NotFound();
        }

        
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id);
            return result ? Ok(new { message = "Cancelled" }) : NotFound();
        }

        
        [Authorize(Roles = "Host,Admin")]
        [HttpGet("guest/{guestId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByGuest(int guestId)
        {
            var bookings = await _bookingService.GetBookingsByGuestIdAsync(guestId);
            return Ok(bookings);
        }

        
        [Authorize(Roles = "Host,Admin")]
        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByProperty(int propertyId)
        {
            var bookings = await _bookingService.GetBookingsByPropertyIdAsync(propertyId);
            return Ok(bookings);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByStatus(BookingStatus status)
        {
            var bookings = await _bookingService.GetBookingsByStatusAsync(status);
            return Ok(bookings);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var bookings = await _bookingService.GetBookingsByDateRangeAsync(startDate, endDate);
            return Ok(bookings);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus status)
        {
            var result = await _bookingService.UpdateBookingStatusAsync(id, status);
            if (!result)
                return NotFound($"Booking with ID {id} not found.");

            return Ok(new { message = "Booking status updated successfully." });
        }
        [Authorize(Roles = "Host")]
        [HttpGet("pending-requests")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetPendingRequests()
        {
            // Kiolvassuk a bejelentkezett Host ID-ját a Claims-bõl
            var hostId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (hostId == 0) return Unauthorized();

            var requests = await _bookingService.GetPendingBookingsForHostAsync(hostId);
            return Ok(requests);
        }
    }
}
