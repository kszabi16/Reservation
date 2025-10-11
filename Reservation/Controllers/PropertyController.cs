using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties() =>
            Ok(await _propertyService.GetAllPropertiesAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetProperty(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            return property is null ? NotFound() : Ok(property);
        }

        [Authorize(Roles = "Host,Admin")]
        [HttpPost]
        public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto dto)
        {
            var property = await _propertyService.CreatePropertyAsync(dto);
            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }

        [Authorize(Roles = "Host,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, [FromBody] CreatePropertyDto dto)
        {
            var property = await _propertyService.UpdatePropertyAsync(id, dto);
            return property is null ? NotFound() : Ok(property);
        }

        [Authorize(Roles = "Host,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            var result = await _propertyService.DeletePropertyAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
