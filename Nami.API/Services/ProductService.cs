using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class ProductService : IProductService
    {
        private readonly DeliveryDbContext _context;

        public ProductService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAll() =>
            await _context.Products
                .Include(p => p.Restaurant)
                .Include(p => p.Category)
                .Select(p => p.ToDto())
                .ToListAsync();

        public async Task<List<ProductDto>> GetByRestaurant(int restaurantId) =>
            await _context.Products
                .Include(p => p.Restaurant)
                .Include(p => p.Category)
                .Where(p => p.RestaurantId == restaurantId)
                .Select(p => p.ToDto())
                .ToListAsync();

        public async Task<ProductDto?> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Restaurant)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            return product?.ToDto();
        }

        public async Task<ProductDto> Create(CreateProductRequest request)
        {
            if (!await _context.Restaurants.AnyAsync(r => r.Id == request.RestaurantId))
                throw new InvalidOperationException("Restaurante no encontrado.");
            if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId))
                throw new InvalidOperationException("Categoría no encontrada.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                RestaurantId = request.RestaurantId,
                CategoryId = request.CategoryId,
                ImageUrl = request.ImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (await GetById(product.Id))!;
        }

        public async Task<ProductDto> Update(int id, UpdateProductRequest request)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new InvalidOperationException("Producto no encontrado.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.CategoryId = request.CategoryId;
            product.ImageUrl = request.ImageUrl;

            await _context.SaveChangesAsync();
            return (await GetById(product.Id))!;
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new InvalidOperationException("Producto no encontrado.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
