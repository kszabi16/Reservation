using Microsoft.AspNetCore.Mvc;
using Reservation.Service.Services;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SmartSearchService _searchService;

        public SearchController(SmartSearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("A keresőszó nem lehet üres.");
            }

            var results = await _searchService.SearchAsync(q);
            return Ok(results);
        }
    }
}