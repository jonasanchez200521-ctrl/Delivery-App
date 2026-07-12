namespace Delivery.API.Models
{
    // Se llama "Courier" (no "Delivery") para no chocar con el namespace del proyecto.
    public class Courier : User
    {
        public VehicleType VehicleType { get; set; }
        public string LicencePlate { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool Availability { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
