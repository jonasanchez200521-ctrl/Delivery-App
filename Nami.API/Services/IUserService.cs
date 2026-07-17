using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Services
{
    public interface IUserService
    {
        Task<List<ClientDto>> GetClients();
        Task<List<DeliveryDto>> GetDeliveries();
        Task<List<DeliveryDto>> GetAvailableDeliveries();
        Task<List<AdministratorDto>> GetAdministrators();

        Task<DeliveryDto> CreateDelivery(CreateDeliveryRequest request);
        Task<AdministratorDto> CreateAdministrator(CreateAdministratorRequest request);

        Task UpdateUserStatus(int userId, UserStatus status);
        Task UpdateDeliveryAvailability(int deliveryId, bool availability);
    }
}
