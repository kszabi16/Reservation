using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties()
        {
            try
            {
                var properties = await _propertyService.GetAllPropertiesAsync();
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetProperty(int id)
        {
            try
            {
                var property = await _propertyService.GetPropertyByIdAsync(id);
                if (property == null)
                    return NotFound($"Property with ID {id} not found.");

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto createPropertyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var property = await _propertyService.CreatePropertyAsync(createPropertyDto);
                return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
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
        public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, [FromBody] CreatePropertyDto updatePropertyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var property = await _propertyService.UpdatePropertyAsync(id, updatePropertyDto);
                if (property == null)
                    return NotFound($"Property with ID {id} not found.");

                return Ok(property);
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
        public async Task<ActionResult> DeleteProperty(int id)
        {
            try
            {
                var result = await _propertyService.DeletePropertyAsync(id);
                if (!result)
                    return NotFound($"Property with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("host/{hostId}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByHost(int hostId)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesByHostIdAsync(hostId);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByLocation(string location)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesByLocationAsync(location);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("price-range")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByPriceRange(
            [FromQuery] decimal minPrice, 
            [FromQuery] decimal maxPrice)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("capacity/{minCapacity}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByCapacity(int minCapacity)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesByCapacityAsync(minCapacity);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
