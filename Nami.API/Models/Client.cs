namespace Nami.API.Models
{
    public class Client : User
    {
        public DateTime RegisterDate { get; set; }

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
