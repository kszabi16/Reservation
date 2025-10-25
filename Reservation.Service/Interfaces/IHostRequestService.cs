using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface IHostRequestService
    {
        Task<IEnumerable<HostRequestDto>> GetAllAsync();
        Task<IEnumerable<HostRequestDto>> GetPendingAsync();
        Task<HostRequestDto?> GetByIdAsync(int id);
        Task<HostRequestDto> CreateAsync(CreateHostRequestDto dto);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id);
    }
}
