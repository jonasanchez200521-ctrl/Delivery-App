using Nami.API.Models;

namespace Nami.API.DTOs
{
    public record UserDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Address,
        Gender Gender,
        DateTime DateBirth,
        UserStatus Status,
        string UserType);

    public record ClientDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Address,
        Gender Gender,
        DateTime DateBirth,
        UserStatus Status,
        DateTime RegisterDate);

    public record DeliveryDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Address,
        Gender Gender,
        DateTime DateBirth,
        UserStatus Status,
        VehicleType VehicleType,
        string LicencePlate,
        double Rating,
        bool Availability);

    public record AdministratorDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Address,
        Gender Gender,
        DateTime DateBirth,
        UserStatus Status,
        string Branch,
        string Rol,
        decimal Salary);

    public record CreateDeliveryRequest(
        string FirstName,
        string LastName,
        string Address,
        string Email,
        string Phone,
        Gender Gender,
        string Password,
        DateTime DateBirth,
        VehicleType VehicleType,
        string LicencePlate);

    public record CreateAdministratorRequest(
        string FirstName,
        string LastName,
        string Address,
        string Email,
        string Phone,
        Gender Gender,
        string Password,
        DateTime DateBirth,
        string Branch,
        string Rol,
        decimal Salary);

    public record UpdateUserStatusRequest(UserStatus Status);

    public record UpdateDeliveryAvailabilityRequest(bool Availability);
}
