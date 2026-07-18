using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly DeliveryDbContext _context;

        public RestaurantService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<RestaurantDto>> GetAll() =>
            await _context.Restaurants.Select(r => r.ToDto()).ToListAsync();

        public async Task<RestaurantDto?> GetById(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            return restaurant?.ToDto();
        }

        public async Task<RestaurantDto> Create(CreateRestaurantRequest request)
        {
            var restaurant = new Restaurant
            {
                Name = request.Name,
                Address = request.Address,
                Category = request.Category,
                Status = RestaurantStatus.Active,
                Rating = 0,
                ImageUrl = request.ImageUrl,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant.ToDto();
        }

        public async Task<RestaurantDto> Update(int id, UpdateRestaurantRequest request)
        {
            var restaurant = await _context.Restaurants.FindAsync(id)
                ?? throw new InvalidOperationException("Restaurante no encontrado.");

            restaurant.Name = request.Name;
            restaurant.Address = request.Address;
            restaurant.Category = request.Category;
            restaurant.Status = request.Status;
            restaurant.Rating = request.Rating;
            restaurant.ImageUrl = request.ImageUrl;
            restaurant.Latitude = request.Latitude;
            restaurant.Longitude = request.Longitude;

            await _context.SaveChangesAsync();
            return restaurant.ToDto();
        }

        public async Task Delete(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id)
                ?? throw new InvalidOperationException("Restaurante no encontrado.");

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}
