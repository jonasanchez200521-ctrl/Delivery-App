using Delivery.API.DTOs;
using Delivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RestaurantDto>>> GetAll() => Ok(await _restaurantService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var restaurant = await _restaurantService.GetById(id);
            return restaurant is null ? NotFound() : Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<RestaurantDto>> Create(CreateRestaurantRequest request) =>
            Ok(await _restaurantService.Create(request));

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<RestaurantDto>> Update(int id, UpdateRestaurantRequest request)
        {
            try
            {
                return Ok(await _restaurantService.Update(id, request));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _restaurantService.Delete(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
