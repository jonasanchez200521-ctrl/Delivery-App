using Nami.API.DTOs;

namespace Nami.API.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAll();
        Task<CategoryDto> Create(CreateCategoryRequest request);
        Task Delete(int id);
    }
}
