using System.Security.Claims;
using Delivery.API.DTOs;
using Delivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authService;

        public AuthController(IAuthentication authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                return Ok(await _authService.Login(request));
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterClientRequest request)
        {
            try
            {
                return Ok(await _authService.Register(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordRequest request)
        {
            var ok = await _authService.RecoverPassword(request.Email);
            return Ok(new { success = ok });
        }

        [HttpPost("unlock-account")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UnlockAccount(UnlockAccountRequest request)
        {
            var ok = await _authService.UnlockAccount(request.Email);
            return Ok(new { success = ok });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _authService.Logout(userId);
            return Ok(new { success = true });
        }
    }
}
