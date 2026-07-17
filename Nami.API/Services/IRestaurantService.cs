using Nami.API.DTOs;

namespace Nami.API.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDto>> GetAll();
        Task<RestaurantDto?> GetById(int id);
        Task<RestaurantDto> Create(CreateRestaurantRequest request);
        Task<RestaurantDto> Update(int id, UpdateRestaurantRequest request);
        Task Delete(int id);
    }
}
