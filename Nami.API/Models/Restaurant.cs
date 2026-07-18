namespace Nami.API.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Categoría/tipo de cocina del restaurante (texto libre, ej. "Comida rápida").
        // No confundir con la entidad Category, que agrupa Products.
        public string Category { get; set; } = string.Empty;
        public RestaurantStatus Status { get; set; } = RestaurantStatus.Active;
        public double Rating { get; set; }
        public string? ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
