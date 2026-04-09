using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
        Task<PropertyDto?> GetPropertyByIdAsync(int id);
        Task<PropertyDto> CreatePropertyAsync(int userId,CreatePropertyDto createPropertyDto);
        Task<PropertyDto?> UpdatePropertyAsync(int id, CreatePropertyDto updatePropertyDto);
        Task<bool> DeletePropertyAsync(int id);
        Task<IEnumerable<PropertyDto>> GetPropertiesByHostIdAsync(int hostId);
        Task<IEnumerable<PropertyDto>> GetAllPropertiesForAdminAsync();
        Task<IEnumerable<PropertyDto>> GetPendingPropertiesAsync();
        Task<bool> ApprovePropertyAsync(int id);
        Task<bool> AddPropertyImagesAsync(int propertyId, List<string> imageUrls);
        Task<IEnumerable<string>> GetAllAmenitiesAsync();
    }
}
