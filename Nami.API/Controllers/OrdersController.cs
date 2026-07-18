using System.Security.Claims;
using Nami.API.DTOs;
using Nami.API.Exceptions;
using Nami.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nami.API.Controllers
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

        [HttpGet("delivery/mine")]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<List<OrderDto>>> GetMyDeliveries() =>
            Ok(await _orderService.GetByDelivery(CurrentUserId));

        [HttpGet("available")]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<List<OrderDto>>> GetAvailable() => Ok(await _orderService.GetAvailable());

        [HttpPost("{id:int}/accept")]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<OrderDto>> Accept(int id)
        {
            try
            {
                return Ok(await _orderService.AcceptOrder(id, CurrentUserId));
            }
            catch (OrderConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id:int}/reject")]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<OrderDto>> Reject(int id)
        {
            try
            {
                return Ok(await _orderService.RejectOrder(id, CurrentUserId));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

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
        [Authorize(Roles = "Administrator,Delivery")]
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

        [HttpPatch("{id:int}/assign-delivery")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<OrderDto>> AssignDelivery(int id, AssignDeliveryRequest request)
        {
            try
            {
                return Ok(await _orderService.AssignDelivery(id, request.DeliveryId));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id:int}/rate-delivery")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<OrderDto>> RateDelivery(int id, RateDeliveryRequest request)
        {
            try
            {
                return Ok(await _orderService.RateDelivery(id, CurrentUserId, request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
