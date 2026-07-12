using Delivery.API.DTOs;
using Delivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<ActionResult<PaymentDto>> GetByOrder(int orderId)
        {
            var payment = await _paymentService.GetByOrder(orderId);
            return payment is null ? NotFound() : Ok(payment);
        }
    }
}
