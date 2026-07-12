using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Services
{
    public interface IPromotionService
    {
        Task<List<PromotionDto>> GetAll();
        Task<Promotion?> GetActiveByCode(string code);
        Task<PromotionDto> Create(CreatePromotionRequest request);
        Task Delete(int id);
    }
}
