using Nami.Web.Models;

namespace Nami.Web.Services
{
    public class AuthApiService
    {
        private readonly ApiClient _api;

        public AuthApiService(ApiClient api)
        {
            _api = api;
        }

        public Task<LoginResponse?> Login(LoginRequest request) =>
            _api.PostAsync<LoginResponse>("api/auth/login", request);

        public Task<UserDto?> Register(RegisterClientRequest request) =>
            _api.PostAsync<UserDto>("api/auth/register", request);

        public Task RecoverPassword(string email) =>
            _api.PostAsync("api/auth/recover-password", new { email });

        public Task UnlockAccount(string email) =>
            _api.PostAsync("api/auth/unlock-account", new UnlockAccountRequest(email));

        public Task Logout() => _api.PostAsync("api/auth/logout", new { });
    }
}
