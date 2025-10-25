using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;

        public PropertyService(IMapper mapper, ReservationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _context.Properties
                .Where(p => p.IsApproved)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            return property == null ? null : _mapper.Map<PropertyDto>(property);
        }

        public async Task<PropertyDto> CreatePropertyAsync(int userId, CreatePropertyDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var property = _mapper.Map<Property>(dto);
            property.HostId = userId;

            // ha Guest adja fel, nem publikáljuk, hanem pending lesz
            if (user.Role == RoleType.Guest)
            {
                property.IsApproved = false;

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                // létrehozunk egy HostRequest-et
                var hostRequest = new HostRequest
                {
                    UserId = userId,
                    PropertyId = property.Id,
                    RequestedAt = DateTime.UtcNow,
                    IsApproved = false
                };
                _context.HostRequests.Add(hostRequest);
                await _context.SaveChangesAsync();
            }
            else
            {
                // ha már Host, azonnal mehet publikálva
                property.IsApproved = true;
                _context.Properties.Add(property);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<PropertyDto>(property);
        }


        public async Task<PropertyDto?> UpdatePropertyAsync(int id, CreatePropertyDto updatePropertyDto)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return null;

            _mapper.Map(updatePropertyDto, property);
            await _context.SaveChangesAsync();

            return _mapper.Map<PropertyDto>(property);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return false;

            property.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByHostIdAsync(int hostId)
        {
            var properties = await _context.Properties
                .Where(p => p.HostId == hostId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByLocationAsync(string location)
        {
            var properties = await _context.Properties
                .Where(p => p.Location.Contains(location))
                .ToListAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var properties = await _context.Properties
                .Where(p => p.PricePerNight >= minPrice && p.PricePerNight <= maxPrice)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByCapacityAsync(int minCapacity)
        {
            var properties = await _context.Properties
                .Where(p => p.Capacity >= minCapacity)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesForAdminAsync()
        {
            var properties = await _context.Properties.ToListAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
    }
}
