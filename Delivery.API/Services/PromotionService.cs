using Delivery.API.Data;
using Delivery.API.DTOs;
using Delivery.API.Mappers;
using Delivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly DeliveryDbContext _context;

        public PromotionService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<PromotionDto>> GetAll() =>
            await _context.Promotions.Select(p => p.ToDto()).ToListAsync();

        public async Task<Promotion?> GetActiveByCode(string code)
        {
            var now = DateTime.UtcNow;
            return await _context.Promotions.FirstOrDefaultAsync(p =>
                p.Code == code && p.IsActive && p.StartDate <= now && p.EndDate >= now);
        }

        public async Task<PromotionDto> Create(CreatePromotionRequest request)
        {
            if (await _context.Promotions.AnyAsync(p => p.Code == request.Code))
                throw new InvalidOperationException("Ya existe una promoción con ese código.");

            var promotion = new Promotion
            {
                Code = request.Code,
                Discount = request.Discount,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion.ToDto();
        }

        public async Task Delete(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id)
                ?? throw new InvalidOperationException("Promoción no encontrada.");

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }
    }
}
