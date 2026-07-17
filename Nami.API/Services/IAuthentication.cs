using Nami.API.DTOs;

namespace Nami.API.Services
{
    // Interfaz de dominio (UML): contrato de autenticación.
    public interface IAuthentication
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<UserDto> Register(RegisterClientRequest request);
        Task<bool> RecoverPassword(string email);
        Task<bool> UnlockAccount(string email);
        Task Logout(int userId);
    }
}
