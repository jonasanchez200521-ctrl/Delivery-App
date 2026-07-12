namespace Delivery.API.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Blocked
    }

    public enum VehicleType
    {
        Bike,
        Motorcycle,
        Car
    }

    public enum RestaurantStatus
    {
        Active,
        Inactive
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        InPreparation,
        ReadyForPickup,
        PickedUp,
        Delivered,
        Cancelled
    }

    public enum PaymentMethod
    {
        Cash,
        Card,
        Transfer
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum NotificationType
    {
        Order,
        Promotion,
        System
    }
}
