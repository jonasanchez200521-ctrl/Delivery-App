using System.Security.Claims;
using Delivery.API.DTOs;
using Delivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderRequest request)
        {
            try
            {
                return Ok(await _orderService.CreateFromCart(CurrentUserId, request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<List<OrderDto>>> GetMine() => Ok(await _orderService.GetByClient(CurrentUserId));

        [HttpGet("courier/mine")]
        [Authorize(Roles = "Courier")]
        public async Task<ActionResult<List<OrderDto>>> GetMyDeliveries() =>
            Ok(await _orderService.GetByCourier(CurrentUserId));

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<OrderDto>>> GetAll() => Ok(await _orderService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetById(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Administrator,Courier")]
        public async Task<ActionResult<OrderDto>> UpdateStatus(int id, UpdateOrderStatusRequest request)
        {
            try
            {
                return Ok(await _orderService.UpdateStatus(id, request));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}/assign-courier")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<OrderDto>> AssignCourier(int id, AssignCourierRequest request)
        {
            try
            {
                return Ok(await _orderService.AssignCourier(id, request.CourierId));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
