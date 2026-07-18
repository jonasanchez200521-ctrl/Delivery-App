namespace Nami.Web.State
{
    // Preferencia de tema en memoria del circuito (se pierde al refrescar, igual que AuthState).
    public class ThemeState
    {
        public string Current { get; private set; } = "dark";

        public event Action? OnChange;

        public void Toggle()
        {
            Current = Current == "dark" ? "light" : "dark";
            OnChange?.Invoke();
        }
    }
}
