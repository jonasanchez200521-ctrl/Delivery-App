using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Mappers
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
                Courier => "Courier",
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

        public static CourierDto ToDto(this Courier courier) => new(
            courier.Id,
            courier.FirstName,
            courier.LastName,
            courier.Email,
            courier.Phone,
            courier.Address,
            courier.Gender,
            courier.DateBirth,
            courier.Status,
            courier.VehicleType,
            courier.LicencePlate,
            courier.Rating,
            courier.Availability);

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
