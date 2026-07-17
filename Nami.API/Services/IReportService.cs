using Nami.API.DTOs;

namespace Nami.API.Services
{
    public interface IReportService
    {
        Task<ReportDto> GenerateReport();
    }
}
