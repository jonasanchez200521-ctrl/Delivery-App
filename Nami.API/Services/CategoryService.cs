using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DeliveryDbContext _context;

        public CategoryService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAll() =>
            await _context.Categories.Select(c => c.ToDto()).ToListAsync();

        public async Task<CategoryDto> Create(CreateCategoryRequest request)
        {
            var category = new Category { Name = request.Name, Description = request.Description };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.ToDto();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id)
                ?? throw new InvalidOperationException("Categoría no encontrada.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
