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
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize]
        [HttpGet("my-bookings")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var bookings = await _bookingService.GetBookingsByGuestIdAsync(userId);
            return Ok(bookings);
        }

        [Authorize]
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

        [Authorize]
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id);
            return result ? Ok(new { message = "Cancelled" }) : NotFound();
        }
    }
}
