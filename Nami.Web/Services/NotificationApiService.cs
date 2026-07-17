using Nami.Web.Models;

namespace Nami.Web.Services
{
    public class NotificationApiService
    {
        private readonly ApiClient _api;

        public NotificationApiService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<NotificationDto>> GetMine() =>
            await _api.GetAsync<List<NotificationDto>>("api/notifications/mine") ?? [];

        public Task MarkAsRead(int id) => _api.PatchAsync($"api/notifications/{id}/read", new { });
    }
}
