namespace Delivery.API.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public decimal Subtotal { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
