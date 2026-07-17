using System.Security.Claims;
using Nami.API.DTOs;
using Nami.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nami.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int ClientId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart() => Ok(await _cartService.GetCart(ClientId));

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItem(AddCartItemRequest request)
        {
            try
            {
                return Ok(await _cartService.AddItem(ClientId, request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("items/{itemId:int}")]
        public async Task<ActionResult<CartDto>> UpdateItem(int itemId, UpdateCartItemRequest request)
        {
            try
            {
                return Ok(await _cartService.UpdateItem(ClientId, itemId, request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("items/{itemId:int}")]
        public async Task<ActionResult<CartDto>> RemoveItem(int itemId)
        {
            try
            {
                return Ok(await _cartService.RemoveItem(ClientId, itemId));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCart(ClientId);
            return NoContent();
        }
    }
}
