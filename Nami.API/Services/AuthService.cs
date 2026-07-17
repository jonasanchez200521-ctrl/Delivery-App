using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class AuthService : IAuthentication
    {
        private readonly DeliveryDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(DeliveryDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
                ?? throw new InvalidOperationException("Credenciales inválidas.");

            if (user.Status != UserStatus.Active)
                throw new InvalidOperationException("La cuenta no está activa. Contacte al administrador.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Credenciales inválidas.");

            var userType = user switch
            {
                Client => "Client",
                Delivery => "Delivery",
                Administrator => "Administrator",
                _ => "Unknown"
            };

            var (token, expiresAt) = _tokenService.GenerateToken(user, userType);
            return new LoginResponse(token, expiresAt, user.ToDto());
        }

        public async Task<UserDto> Register(RegisterClientRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Ya existe una cuenta con ese correo.");

            var client = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                Gender = request.Gender,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                DateBirth = request.DateBirth,
                Status = UserStatus.Active,
                RegisterDate = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            _context.Carts.Add(new Cart { ClientId = client.Id, Subtotal = 0 });
            await _context.SaveChangesAsync();

            return ((User)client).ToDto();
        }

        public async Task<bool> RecoverPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            // Simulado: no se envía correo real, se registra una notificación in-app.
            _context.Notifications.Add(new Notification
            {
                UserId = user.Id,
                Message = "Se solicitó la recuperación de tu contraseña. Contacta a soporte para continuar.",
                Type = NotificationType.System,
                IsRead = false,
                Date = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlockAccount(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            user.Status = UserStatus.Active;
            await _context.SaveChangesAsync();
            return true;
        }

        public Task Logout(int userId) => Task.CompletedTask; // JWT es stateless: el cliente descarta el token.
    }
}
