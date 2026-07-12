using Delivery.API.DTOs;

namespace Delivery.API.Services
{
    public interface IReportService
    {
        Task<ReportDto> GenerateReport();
    }
}
