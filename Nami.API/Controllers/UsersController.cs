using System.Security.Claims;
using Nami.API.DTOs;
using Nami.API.Models;
using Nami.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nami.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("clients")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<ClientDto>>> GetClients() => Ok(await _userService.GetClients());

        [HttpGet("deliveries")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<DeliveryDto>>> GetDeliveries() => Ok(await _userService.GetDeliveries());

        [HttpGet("deliveries/available")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<DeliveryDto>>> GetAvailableDeliveries() =>
            Ok(await _userService.GetAvailableDeliveries());

        [HttpGet("administrators")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<AdministratorDto>>> GetAdministrators() =>
            Ok(await _userService.GetAdministrators());

        [HttpPost("deliveries")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<DeliveryDto>> CreateDelivery(CreateDeliveryRequest request)
        {
            try
            {
                return Ok(await _userService.CreateDelivery(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("administrators")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<AdministratorDto>> CreateAdministrator(CreateAdministratorRequest request)
        {
            try
            {
                return Ok(await _userService.CreateAdministrator(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateUserStatusRequest request)
        {
            await _userService.UpdateUserStatus(id, request.Status);
            return NoContent();
        }

        [HttpPatch("deliveries/{id:int}/availability")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateDeliveryAvailability(int id, UpdateDeliveryAvailabilityRequest request)
        {
            await _userService.UpdateDeliveryAvailability(id, request.Availability);
            return NoContent();
        }

        [HttpPatch("deliveries/me/availability")]
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> UpdateMyAvailability(UpdateDeliveryAvailabilityRequest request)
        {
            var deliveryId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.UpdateDeliveryAvailability(deliveryId, request.Availability);
            return NoContent();
        }
    }
}
