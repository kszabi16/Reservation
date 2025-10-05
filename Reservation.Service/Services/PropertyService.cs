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
            var properties = await _context.Properties.ToListAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            return property == null ? null : _mapper.Map<PropertyDto>(property);
        }

        public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto)
        {
            var host = await _context.Users.FindAsync(createPropertyDto.HostId);
            if (host == null)
                throw new InvalidOperationException("Host not found.");

            if (host.Role != RoleType.Host && host.Role != RoleType.Admin)
                throw new InvalidOperationException("User is not authorized to create properties.");

            var property = _mapper.Map<Property>(createPropertyDto);
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return _mapper.Map<PropertyDto>(property);
        }

        public async Task<PropertyDto?> UpdatePropertyAsync(int id, CreatePropertyDto updatePropertyDto)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return null;

            var host = await _context.Users.FindAsync(updatePropertyDto.HostId);
            if (host == null)
                throw new InvalidOperationException("Host not found.");

            if (host.Role != RoleType.Host && host.Role != RoleType.Admin)
                throw new InvalidOperationException("User is not authorized to manage properties.");

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
    }
}
