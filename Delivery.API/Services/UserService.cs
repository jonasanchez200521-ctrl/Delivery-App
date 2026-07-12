using Delivery.API.Data;
using Delivery.API.DTOs;
using Delivery.API.Mappers;
using Delivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Services
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

        public async Task<List<CourierDto>> GetCouriers() =>
            await _context.Couriers.Select(c => c.ToDto()).ToListAsync();

        public async Task<List<CourierDto>> GetAvailableCouriers() =>
            await _context.Couriers
                .Where(c => c.Availability && c.Status == UserStatus.Active)
                .Select(c => c.ToDto())
                .ToListAsync();

        public async Task<List<AdministratorDto>> GetAdministrators() =>
            await _context.Administrators.Select(a => a.ToDto()).ToListAsync();

        public async Task<CourierDto> CreateCourier(CreateCourierRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Ya existe una cuenta con ese correo.");

            var courier = new Courier
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

            _context.Couriers.Add(courier);
            await _context.SaveChangesAsync();
            return courier.ToDto();
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

        public async Task UpdateCourierAvailability(int courierId, bool availability)
        {
            var courier = await _context.Couriers.FindAsync(courierId)
                ?? throw new InvalidOperationException("Repartidor no encontrado.");
            courier.Availability = availability;
            await _context.SaveChangesAsync();
        }
    }
}
