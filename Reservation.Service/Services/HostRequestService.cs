using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class HostRequestService : IHostRequestService
    {
        private readonly ReservationDbContext _context;
        private readonly IMapper _mapper;

        public HostRequestService(ReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HostRequestDto>> GetAllAsync()
        {
            var requests = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HostRequestDto>>(requests);
        }

        public async Task<IEnumerable<HostRequestDto>> GetPendingAsync()
        {
            var pending = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .Where(r => !r.IsApproved)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HostRequestDto>>(pending);
        }
        public async Task<bool> ApproveAsync(int id)
        {
            var request = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return false;

            request.IsApproved = true;
            request.Property.IsApproved = true;
            request.ApprovedAt = DateTime.UtcNow;
            request.User.Role = RoleType.Host;

            await _context.SaveChangesAsync();
            return true;
        }
     public async Task<bool> RejectAsync(int id)
        {
            var request = await _context.HostRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return false;

            _context.HostRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
