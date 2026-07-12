using Delivery.Web.Models;

namespace Delivery.Web.Services
{
    public class ReportApiService
    {
        private readonly ApiClient _api;

        public ReportApiService(ApiClient api)
        {
            _api = api;
        }

        public Task<ReportDto?> GetReport() => _api.GetAsync<ReportDto>("api/reports");
    }
}
