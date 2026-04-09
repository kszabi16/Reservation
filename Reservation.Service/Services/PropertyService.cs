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
                .Include(p=>p.Ratings)
                .Where(p => p.IsApproved)
                .Include(p => p.PropertyAmenities)
                .ThenInclude(pa => pa.Amenity)
                    .Include(p => p.Images)
                    .AsSplitQuery()
                .ToListAsync();

            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Ratings)
                .Include(p => p.Images)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == id);

            return property == null ? null : _mapper.Map<PropertyDto>(property);
        }

        public async Task<PropertyDto> CreatePropertyAsync(int userId, CreatePropertyDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var property = _mapper.Map<Property>(dto);
            property.HostId = userId;

            if (user.Role == RoleType.Guest)
            {

                property.IsApproved = false;

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

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
            else if (user.Role == RoleType.Host && !user.IsTrustedHost)
            {

                property.IsApproved = false;

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();
            }
            else
            {

                property.IsApproved = true;

                _context.Properties.Add(property);
               
            }

            await _context.SaveChangesAsync();

            if (dto.Amenities != null && dto.Amenities.Any())
            {
                var existingAmenities = await _context.Amenities
                    .Where(a => dto.Amenities.Contains(a.Name))
                    .ToListAsync();

                foreach (var amenity in existingAmenities)
                {
                    _context.PropertyAmenities.Add(new PropertyAmenity
                    {
                        PropertyId = property.Id,
                        AmenityId = amenity.Id
                    });
                }
                await _context.SaveChangesAsync();
            }
            var createdProperty = await GetPropertyByIdAsync(property.Id);
            return createdProperty!;
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

        public async Task<IEnumerable<PropertyDto>> GetPendingPropertiesAsync()
        {
            var pendingProperties = await _context.Properties
                .Where(p => !p.IsApproved)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PropertyDto>>(pendingProperties);
        }

        public async Task<bool> ApprovePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return false;

            property.IsApproved = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddPropertyImagesAsync(int propertyId, List<string> imageUrls)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return false;

            
            foreach (var url in imageUrls)
            {
                var propertyImage = new PropertyImage
                {
                    ImageUrl = url,
                    PropertyId = propertyId
                };
                _context.PropertyImages.Add(propertyImage);
            }

         
            if (string.IsNullOrEmpty(property.ImageUrl) && imageUrls.Any())
            {
                property.ImageUrl = imageUrls.First();
            }

            await _context.SaveChangesAsync();
            return true;
        }
         public async Task<IEnumerable<string>> GetAllAmenitiesAsync()
        {
            return await _context.Amenities
                .Select(a => a.Name)
                .ToListAsync();
        }
    }
}
