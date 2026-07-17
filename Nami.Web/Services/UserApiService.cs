using Nami.Web.Models;

namespace Nami.Web.Services
{
    public class UserApiService
    {
        private readonly ApiClient _api;

        public UserApiService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<ClientDto>> GetClients() => await _api.GetAsync<List<ClientDto>>("api/users/clients") ?? [];

        public async Task<List<DeliveryDto>> GetDeliveries() => await _api.GetAsync<List<DeliveryDto>>("api/users/deliveries") ?? [];

        public async Task<List<DeliveryDto>> GetAvailableDeliveries() =>
            await _api.GetAsync<List<DeliveryDto>>("api/users/deliveries/available") ?? [];

        public async Task<List<AdministratorDto>> GetAdministrators() =>
            await _api.GetAsync<List<AdministratorDto>>("api/users/administrators") ?? [];

        public Task<DeliveryDto?> CreateDelivery(CreateDeliveryRequest request) =>
            _api.PostAsync<DeliveryDto>("api/users/deliveries", request);

        public Task<AdministratorDto?> CreateAdministrator(CreateAdministratorRequest request) =>
            _api.PostAsync<AdministratorDto>("api/users/administrators", request);

        public Task UpdateStatus(int userId, UpdateUserStatusRequest request) =>
            _api.PatchAsync($"api/users/{userId}/status", request);

        public Task UpdateDeliveryAvailability(int deliveryId, UpdateDeliveryAvailabilityRequest request) =>
            _api.PatchAsync($"api/users/deliveries/{deliveryId}/availability", request);

        public Task UpdateMyAvailability(UpdateDeliveryAvailabilityRequest request) =>
            _api.PatchAsync("api/users/deliveries/me/availability", request);
    }
}
