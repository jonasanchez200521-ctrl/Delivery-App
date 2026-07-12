namespace Delivery.API.Models
{
    public class Administrator : User
    {
        public string Branch { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
