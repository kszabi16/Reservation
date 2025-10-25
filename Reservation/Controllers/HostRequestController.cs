using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostRequestController : ControllerBase
    {
        private readonly IHostRequestService _service;

        public HostRequestController(IHostRequestService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HostRequestDto>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<HostRequestDto>>> GetPending() =>
            Ok(await _service.GetPendingAsync());

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<HostRequestDto>> Create([FromBody] CreateHostRequestDto dto)
        {
            var request = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = request.Id }, request);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> Approve(int id)
        {
            var result = await _service.ApproveAsync(id);
            return result ? Ok(new { message = "Host request approved." }) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/reject")]
        public async Task<ActionResult> Reject(int id)
        {
            var result = await _service.RejectAsync(id);
            return result ? Ok(new { message = "Host request rejected." }) : NotFound();
        }
    }
}
