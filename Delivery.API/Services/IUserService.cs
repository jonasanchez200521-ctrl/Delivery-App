using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Services
{
    public interface IUserService
    {
        Task<List<ClientDto>> GetClients();
        Task<List<CourierDto>> GetCouriers();
        Task<List<CourierDto>> GetAvailableCouriers();
        Task<List<AdministratorDto>> GetAdministrators();

        Task<CourierDto> CreateCourier(CreateCourierRequest request);
        Task<AdministratorDto> CreateAdministrator(CreateAdministratorRequest request);

        Task UpdateUserStatus(int userId, UserStatus status);
        Task UpdateCourierAvailability(int courierId, bool availability);
    }
}
