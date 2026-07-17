using Nami.API.Patterns.Reports;

namespace Nami.API.Models
{
    // Clase base para TPH (Table Per Hierarchy): Client, Delivery, Administrator
    // comparten la tabla "Users" con un discriminador "UserType".
    public abstract class User : IVisitable
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime DateBirth { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public string FullName => $"{FirstName} {LastName}";

        // Patrón Visitor: cada tipo concreto de usuario acepta el visitor
        // y este decide cómo acumular estadísticas según el tipo real (polimorfismo).
        public void Accept(IReportVisitor visitor) => visitor.Visit(this);
    }
}
