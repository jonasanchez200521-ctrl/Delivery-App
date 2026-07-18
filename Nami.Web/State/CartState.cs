namespace Nami.Web.State
{
    // Contador de items del carrito compartido entre páginas, para que el navbar
    // pueda mostrar el badge sin recargar. Vive en memoria del circuito, como AuthState.
    public class CartState
    {
        public int ItemCount { get; private set; }
        public int BumpToken { get; private set; }

        public event Action? OnChange;

        public void SetCount(int count)
        {
            ItemCount = count;
            OnChange?.Invoke();
        }

        public void Bump(int newCount)
        {
            ItemCount = newCount;
            BumpToken++;
            OnChange?.Invoke();
        }
    }
}
