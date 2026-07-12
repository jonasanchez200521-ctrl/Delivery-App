using System.Security.Claims;
using Delivery.API.DTOs;
using Delivery.API.Models;
using Delivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
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

        [HttpGet("couriers")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<CourierDto>>> GetCouriers() => Ok(await _userService.GetCouriers());

        [HttpGet("couriers/available")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<CourierDto>>> GetAvailableCouriers() =>
            Ok(await _userService.GetAvailableCouriers());

        [HttpGet("administrators")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<AdministratorDto>>> GetAdministrators() =>
            Ok(await _userService.GetAdministrators());

        [HttpPost("couriers")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<CourierDto>> CreateCourier(CreateCourierRequest request)
        {
            try
            {
                return Ok(await _userService.CreateCourier(request));
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

        [HttpPatch("couriers/{id:int}/availability")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCourierAvailability(int id, UpdateCourierAvailabilityRequest request)
        {
            await _userService.UpdateCourierAvailability(id, request.Availability);
            return NoContent();
        }

        [HttpPatch("couriers/me/availability")]
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> UpdateMyAvailability(UpdateCourierAvailabilityRequest request)
        {
            var courierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.UpdateCourierAvailability(courierId, request.Availability);
            return NoContent();
        }
    }
}
