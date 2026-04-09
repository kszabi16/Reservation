using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RatingDto>> RateProperty([FromBody] CreateRatingDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var rating = await _ratingService.AddOrUpdateRatingAsync(userId, dto);
                return Ok(rating);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("property/{propertyId}/average")]
        public async Task<ActionResult<double>> GetAverageRating(int propertyId)
        {
            var average = await _ratingService.GetAverageRatingForPropertyAsync(propertyId);
            return Ok(average);
        }
    }
}