using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class UserService : IUserService
    {
        private readonly DeliveryDbContext _context;

        public UserService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientDto>> GetClients() =>
            await _context.Clients.Select(c => c.ToDto()).ToListAsync();

        public async Task<List<DeliveryDto>> GetDeliveries() =>
            await _context.Deliveries.Select(c => c.ToDto()).ToListAsync();

        public async Task<List<DeliveryDto>> GetAvailableDeliveries() =>
            await _context.Deliveries
                .Where(c => c.Availability && c.Status == UserStatus.Active)
                .Select(c => c.ToDto())
                .ToListAsync();

        public async Task<List<AdministratorDto>> GetAdministrators() =>
            await _context.Administrators.Select(a => a.ToDto()).ToListAsync();

        public async Task<DeliveryDto> CreateDelivery(CreateDeliveryRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Ya existe una cuenta con ese correo.");

            var delivery = new Delivery
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
                VehicleType = request.VehicleType,
                LicencePlate = request.LicencePlate,
                Rating = 0,
                Availability = true
            };

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
            return delivery.ToDto();
        }

        public async Task<AdministratorDto> CreateAdministrator(CreateAdministratorRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Ya existe una cuenta con ese correo.");

            var admin = new Administrator
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
                Branch = request.Branch,
                Rol = request.Rol,
                Salary = request.Salary
            };

            _context.Administrators.Add(admin);
            await _context.SaveChangesAsync();
            return admin.ToDto();
        }

        public async Task UpdateUserStatus(int userId, UserStatus status)
        {
            var user = await _context.Users.FindAsync(userId)
                ?? throw new InvalidOperationException("Usuario no encontrado.");
            user.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryAvailability(int deliveryId, bool availability)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId)
                ?? throw new InvalidOperationException("Repartidor no encontrado.");
            delivery.Availability = availability;
            await _context.SaveChangesAsync();
        }
    }
}
