namespace Nami.Web.Models
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

    public record UnlockAccountRequest(string Email);

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
}
