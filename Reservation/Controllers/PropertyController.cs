using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;
using System.Security.Claims;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

     
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetProperty(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound($"Property with ID {id} not found.");
            return Ok(property);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto createPropertyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var property = await _propertyService.CreatePropertyAsync(userId,createPropertyDto);
            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }

        [Authorize(Roles = "Host,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, [FromBody] CreatePropertyDto updatePropertyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var property = await _propertyService.UpdatePropertyAsync(id, updatePropertyDto);
            if (property == null)
                return NotFound($"Property with ID {id} not found.");

            return Ok(property);
        }

        
        [Authorize(Roles = "Host,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            var result = await _propertyService.DeletePropertyAsync(id);
            if (!result)
                return NotFound($"Property with ID {id} not found.");

            return NoContent();
        }

        
        [Authorize(Roles = "Host,Admin")]
        [HttpGet("host/{hostId}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByHost(int hostId)
        {
            var properties = await _propertyService.GetPropertiesByHostIdAsync(hostId);
            return Ok(properties);
        }

        
        [AllowAnonymous]
        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByLocation(string location)
        {
            var properties = await _propertyService.GetPropertiesByLocationAsync(location);
            return Ok(properties);
        }

        [AllowAnonymous]
        [HttpGet("price-range")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            var properties = await _propertyService.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);
            return Ok(properties);
        }

        
        [AllowAnonymous]
        [HttpGet("capacity/{minCapacity}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByCapacity(int minCapacity)
        {
            var properties = await _propertyService.GetPropertiesByCapacityAsync(minCapacity);
            return Ok(properties);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllPropertiesForAdmin()
        {
            var properties = await _propertyService.GetAllPropertiesForAdminAsync();
            return Ok(properties);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPendingProperties()
        {
            var properties = await _propertyService.GetPendingPropertiesAsync();
            return Ok(properties);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveProperty(int id)
        {
            var success = await _propertyService.ApprovePropertyAsync(id);
            if (!success)
                return NotFound(new { message = "Ingatlan nem található." });

            return Ok(new { message = "Szállás sikeresen jóváhagyva!" });
        }

    }
}
