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

        // Admin: összes HostRequest
        public async Task<IEnumerable<HostRequestDto>> GetAllAsync()
        {
            var requests = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HostRequestDto>>(requests);
        }

        // Admin: függőben lévők
        public async Task<IEnumerable<HostRequestDto>> GetPendingAsync()
        {
            var pending = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .Where(r => !r.IsApproved)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HostRequestDto>>(pending);
        }

        // Egy adott request lekérdezése
        public async Task<HostRequestDto?> GetByIdAsync(int id)
        {
            var request = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .FirstOrDefaultAsync(r => r.Id == id);

            return request == null ? null : _mapper.Map<HostRequestDto>(request);
        }

        //  Új HostRequest létrehozása (Guest indítja automatikusan)
        public async Task<HostRequestDto> CreateAsync(CreateHostRequestDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            var property = await _context.Properties.FindAsync(dto.PropertyId);

            if (user == null)
                throw new InvalidOperationException("User not found.");
            if (property == null)
                throw new InvalidOperationException("Property not found.");

            var newRequest = _mapper.Map<HostRequest>(dto);
            _context.HostRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            // újra betöltjük a kapcsolt adatokat a DTO-hoz
            var loaded = await _context.HostRequests
                .Include(r => r.User)
                .Include(r => r.Property)
                .FirstAsync(r => r.Id == newRequest.Id);

            return _mapper.Map<HostRequestDto>(loaded);
        }

        // Admin jóváhagyás
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

        // Admin elutasítás
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
