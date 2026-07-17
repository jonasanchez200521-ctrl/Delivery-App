using Nami.API.DTOs;

namespace Nami.API.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAll();
        Task<List<ProductDto>> GetByRestaurant(int restaurantId);
        Task<ProductDto?> GetById(int id);
        Task<ProductDto> Create(CreateProductRequest request);
        Task<ProductDto> Update(int id, UpdateProductRequest request);
        Task Delete(int id);
    }
}
