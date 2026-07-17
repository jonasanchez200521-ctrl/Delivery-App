using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Services
{
    public interface IPromotionService
    {
        Task<List<PromotionDto>> GetAll();
        Task<Promotion?> GetActiveByCode(string code);
        Task<PromotionDto> Create(CreatePromotionRequest request);
        Task Delete(int id);
    }
}
