using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;

namespace Reservation.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SystemLogController : ControllerBase
    {
        private readonly ReservationDbContext _context;
        private readonly IMapper _mapper;

        public SystemLogController(ReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemLogDto>>> GetLogs()
        {
            var logs = await _context.SystemLogs
                .OrderByDescending(l => l.CreatedAt)
                .Take(100)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<SystemLogDto>>(logs));
        }
    }
}