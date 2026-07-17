using Nami.Web.Models;

namespace Nami.Web.State
{
    // Estado de sesión en memoria, con ciclo de vida Scoped (uno por circuito de Blazor Server).
    // Se pierde al refrescar el navegador: aceptable para el alcance de este proyecto universitario.
    public class AuthState
    {
        public string? Token { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public UserDto? CurrentUser { get; private set; }

        public bool IsAuthenticated => Token is not null && ExpiresAt > DateTime.UtcNow;
        public string Role => CurrentUser?.UserType ?? string.Empty;

        public event Action? OnChange;

        public void SetSession(LoginResponse response)
        {
            Token = response.Token;
            ExpiresAt = response.ExpiresAt;
            CurrentUser = response.User;
            OnChange?.Invoke();
        }

        public void Clear()
        {
            Token = null;
            ExpiresAt = null;
            CurrentUser = null;
            OnChange?.Invoke();
        }
    }
}
