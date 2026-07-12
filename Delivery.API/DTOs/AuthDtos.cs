using Delivery.API.Models;

namespace Delivery.API.DTOs
{
    public record LoginRequest(string Email, string Password);

    public record LoginResponse(string Token, DateTime ExpiresAt, UserDto User);

    public record RegisterClientRequest(
        string FirstName,
        string LastName,
        string Address,
        string Email,
        string Phone,
        Gender Gender,
        string Password,
        DateTime DateBirth);

    public record RecoverPasswordRequest(string Email);

    public record UnlockAccountRequest(string Email);
}
