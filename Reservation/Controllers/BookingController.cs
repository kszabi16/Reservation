using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound($"Booking with ID {id} not found.");

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var booking = await _bookingService.CreateBookingAsync(createBookingDto);
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingDto>> UpdateBooking(int id, [FromBody] CreateBookingDto updateBookingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var booking = await _bookingService.UpdateBookingAsync(id, updateBookingDto);
                if (booking == null)
                    return NotFound($"Booking with ID {id} not found.");

                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            try
            {
                var result = await _bookingService.DeleteBookingAsync(id);
                if (!result)
                    return NotFound($"Booking with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("guest/{guestId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByGuest(int guestId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByGuestIdAsync(guestId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByProperty(int propertyId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByPropertyIdAsync(propertyId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByStatus(BookingStatus status)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByStatusAsync(status);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByDateRangeAsync(startDate, endDate);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus status)
        {
            try
            {
                var result = await _bookingService.UpdateBookingStatusAsync(id, status);
                if (!result)
                    return NotFound($"Booking with ID {id} not found.");

                return Ok(new { message = "Booking status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            try
            {
                var result = await _bookingService.CancelBookingAsync(id);
                if (!result)
                    return NotFound($"Booking with ID {id} not found.");

                return Ok(new { message = "Booking cancelled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/confirm")]
        public async Task<ActionResult> ConfirmBooking(int id)
        {
            try
            {
                var result = await _bookingService.ConfirmBookingAsync(id);
                if (!result)
                    return NotFound($"Booking with ID {id} not found.");

                return Ok(new { message = "Booking confirmed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
