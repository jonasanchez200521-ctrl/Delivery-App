using Delivery.Web.Models;

namespace Delivery.Web.Services
{
    public class UserApiService
    {
        private readonly ApiClient _api;

        public UserApiService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<ClientDto>> GetClients() => await _api.GetAsync<List<ClientDto>>("api/users/clients") ?? [];

        public async Task<List<CourierDto>> GetCouriers() => await _api.GetAsync<List<CourierDto>>("api/users/couriers") ?? [];

        public async Task<List<CourierDto>> GetAvailableCouriers() =>
            await _api.GetAsync<List<CourierDto>>("api/users/couriers/available") ?? [];

        public async Task<List<AdministratorDto>> GetAdministrators() =>
            await _api.GetAsync<List<AdministratorDto>>("api/users/administrators") ?? [];

        public Task<CourierDto?> CreateCourier(CreateCourierRequest request) =>
            _api.PostAsync<CourierDto>("api/users/couriers", request);

        public Task<AdministratorDto?> CreateAdministrator(CreateAdministratorRequest request) =>
            _api.PostAsync<AdministratorDto>("api/users/administrators", request);

        public Task UpdateStatus(int userId, UpdateUserStatusRequest request) =>
            _api.PatchAsync($"api/users/{userId}/status", request);

        public Task UpdateCourierAvailability(int courierId, UpdateCourierAvailabilityRequest request) =>
            _api.PatchAsync($"api/users/couriers/{courierId}/availability", request);

        public Task UpdateMyAvailability(UpdateCourierAvailabilityRequest request) =>
            _api.PatchAsync("api/users/couriers/me/availability", request);
    }
}
