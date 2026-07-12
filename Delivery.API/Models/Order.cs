using Delivery.API.Patterns.Reports;

namespace Delivery.API.Models
{
    public class Order : IVisitable
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int? CourierId { get; set; }
        public Courier? Courier { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public decimal Total { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Address { get; set; } = string.Empty;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public Payment? Payment { get; set; }

        public void Accept(IReportVisitor visitor) => visitor.Visit(this);
    }
}
