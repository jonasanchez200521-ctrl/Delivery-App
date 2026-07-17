using Nami.Web.Models;

namespace Nami.Web.Services
{
    public class PromotionApiService
    {
        private readonly ApiClient _api;

        public PromotionApiService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<PromotionDto>> GetAll() => await _api.GetAsync<List<PromotionDto>>("api/promotions") ?? [];

        public Task<PromotionDto?> Create(CreatePromotionRequest request) =>
            _api.PostAsync<PromotionDto>("api/promotions", request);

        public Task Delete(int id) => _api.DeleteAsync($"api/promotions/{id}");
    }
}
