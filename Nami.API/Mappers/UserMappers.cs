using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToDto(this User user) => new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Phone,
            user.Address,
            user.Gender,
            user.DateBirth,
            user.Status,
            user switch
            {
                Client => "Client",
                Delivery => "Delivery",
                Administrator => "Administrator",
                _ => "Unknown"
            });

        public static ClientDto ToDto(this Client client) => new(
            client.Id,
            client.FirstName,
            client.LastName,
            client.Email,
            client.Phone,
            client.Address,
            client.Gender,
            client.DateBirth,
            client.Status,
            client.RegisterDate);

        public static DeliveryDto ToDto(this Delivery delivery) => new(
            delivery.Id,
            delivery.FirstName,
            delivery.LastName,
            delivery.Email,
            delivery.Phone,
            delivery.Address,
            delivery.Gender,
            delivery.DateBirth,
            delivery.Status,
            delivery.VehicleType,
            delivery.LicencePlate,
            delivery.Rating,
            delivery.Availability);

        public static AdministratorDto ToDto(this Administrator admin) => new(
            admin.Id,
            admin.FirstName,
            admin.LastName,
            admin.Email,
            admin.Phone,
            admin.Address,
            admin.Gender,
            admin.DateBirth,
            admin.Status,
            admin.Branch,
            admin.Rol,
            admin.Salary);
    }
}
