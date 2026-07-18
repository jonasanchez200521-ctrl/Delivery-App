using Nami.Web.Models;

namespace Nami.Web.Support
{
    // Mapeo puramente visual de OrderStatus a etiqueta/color de badge para el tema Ñami.
    public static class OrderStatusStyle
    {
        public static string Label(OrderStatus status) => status switch
        {
            OrderStatus.Pending => "Pendiente",
            OrderStatus.Confirmed => "Confirmado",
            OrderStatus.InPreparation => "En preparación",
            OrderStatus.ReadyForPickup => "Listo",
            OrderStatus.PickedUp => "Recogido",
            OrderStatus.Delivered => "Entregado",
            OrderStatus.Cancelled => "Cancelado",
            _ => status.ToString()
        };

        public static string Bg(OrderStatus status) => status switch
        {
            OrderStatus.Delivered => "rgba(74,222,128,.18)",
            OrderStatus.Cancelled => "rgba(248,113,113,.18)",
            OrderStatus.Pending => "var(--nami-ph2)",
            _ => "var(--nami-accent-soft)"
        };

        public static string Fg(OrderStatus status) => status switch
        {
            OrderStatus.Delivered => "var(--nami-success)",
            OrderStatus.Cancelled => "var(--nami-danger)",
            OrderStatus.Pending => "var(--nami-muted)",
            _ => "var(--nami-accent)"
        };
    }
}
