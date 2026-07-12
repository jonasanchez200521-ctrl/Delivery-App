using Delivery.API.DTOs;

namespace Delivery.API.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAll();
        Task<CategoryDto> Create(CreateCategoryRequest request);
        Task Delete(int id);
    }
}
