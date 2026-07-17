using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Data
{
    // Datos semilla para que la demo tenga contenido desde el primer arranque.
    public static class DeliverySeed
    {
        // Hashes BCrypt precalculados y fijos: HasData exige valores estáticos y
        // deterministas (BCrypt.HashPassword genera un salt distinto en cada llamada).
        // Admin123!    -> AdminPasswordHash
        // Cliente123!  -> ClientPasswordHash
        // Courier123!  -> DeliveryPasswordHash
        private const string AdminPasswordHash = "$2a$11$qNYoYQeSotuFui857iiB1OuJt1QtqARggr8IYRKZYEVj.3eAiKCXG";
        private const string ClientPasswordHash = "$2a$11$hvX4IFoZ1UAZLzzBWcXAIOlJCy7Gk/BQqtC9j3s4Fk5PXIxfLTH02";
        private const string DeliveryPasswordHash = "$2a$11$8xLd3G.dMRqKGFsYHyaVp.HyA7sbThqUKlyV2aMTkGtKrP6hp1dGa";

        public static void Seed(ModelBuilder modelBuilder)
        {
            var fixedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Administrator>().HasData(new Administrator
            {
                Id = 1,
                FirstName = "Ana",
                LastName = "Torres",
                Address = "Av. Principal 100",
                Email = "admin@delivery.com",
                Phone = "0999999999",
                Gender = Gender.Female,
                PasswordHash = AdminPasswordHash,
                DateBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = UserStatus.Active,
                Branch = "Matriz Quito",
                Rol = "SuperAdmin",
                Salary = 1200m
            });

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 2,
                    FirstName = "Carlos",
                    LastName = "Pérez",
                    Address = "Calle Los Álamos 45",
                    Email = "cliente1@delivery.com",
                    Phone = "0988888881",
                    Gender = Gender.Male,
                    PasswordHash = ClientPasswordHash,
                    DateBirth = new DateTime(1995, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                    Status = UserStatus.Active,
                    RegisterDate = fixedDate
                },
                new Client
                {
                    Id = 3,
                    FirstName = "María",
                    LastName = "López",
                    Address = "Av. Amazonas 210",
                    Email = "cliente2@delivery.com",
                    Phone = "0988888882",
                    Gender = Gender.Female,
                    PasswordHash = ClientPasswordHash,
                    DateBirth = new DateTime(1998, 8, 20, 0, 0, 0, DateTimeKind.Utc),
                    Status = UserStatus.Active,
                    RegisterDate = fixedDate
                });

            modelBuilder.Entity<Delivery>().HasData(
                new Delivery
                {
                    Id = 4,
                    FirstName = "Jorge",
                    LastName = "Ramírez",
                    Address = "Sector La Floresta",
                    Email = "courier1@delivery.com",
                    Phone = "0977777771",
                    Gender = Gender.Male,
                    PasswordHash = DeliveryPasswordHash,
                    DateBirth = new DateTime(1993, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    Status = UserStatus.Active,
                    VehicleType = VehicleType.Motorcycle,
                    LicencePlate = "PBX-1234",
                    Rating = 4.5,
                    Availability = true
                },
                new Delivery
                {
                    Id = 5,
                    FirstName = "Lucía",
                    LastName = "Fernández",
                    Address = "Sector El Bosque",
                    Email = "courier2@delivery.com",
                    Phone = "0977777772",
                    Gender = Gender.Female,
                    PasswordHash = DeliveryPasswordHash,
                    DateBirth = new DateTime(1996, 11, 2, 0, 0, 0, DateTimeKind.Utc),
                    Status = UserStatus.Active,
                    VehicleType = VehicleType.Bike,
                    LicencePlate = "PBX-5678",
                    Rating = 4.8,
                    Availability = true
                });

            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { Id = 1, Name = "La Parrilla Criolla", Address = "Calle 10 y 5", Category = "Comida típica", Status = RestaurantStatus.Active, Rating = 4.6 },
                new Restaurant { Id = 2, Name = "Pizza Nostra", Address = "Av. Amazonas 234", Category = "Pizzería", Status = RestaurantStatus.Active, Rating = 4.3 },
                new Restaurant { Id = 3, Name = "Sushi Zen", Address = "Av. 6 de Diciembre 500", Category = "Comida japonesa", Status = RestaurantStatus.Active, Rating = 4.7 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Platos fuertes", Description = "Platos principales del menú" },
                new Category { Id = 2, Name = "Bebidas", Description = "Bebidas frías y calientes" },
                new Category { Id = 3, Name = "Postres", Description = "Postres y dulces" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Churrasco Mixto", Description = "Carne, huevo, plátano y arroz", Price = 8.50m, Stock = 50, RestaurantId = 1, CategoryId = 1 },
                new Product { Id = 2, Name = "Jugo Natural", Description = "Jugo de fruta natural 500ml", Price = 2.00m, Stock = 100, RestaurantId = 1, CategoryId = 2 },
                new Product { Id = 3, Name = "Pizza Margarita", Description = "Pizza clásica de mozzarella y albahaca", Price = 9.00m, Stock = 40, RestaurantId = 2, CategoryId = 1 },
                new Product { Id = 4, Name = "Gaseosa 500ml", Description = "Bebida gaseosa", Price = 1.50m, Stock = 100, RestaurantId = 2, CategoryId = 2 },
                new Product { Id = 5, Name = "Sushi Roll California", Description = "8 piezas de roll california", Price = 7.50m, Stock = 30, RestaurantId = 3, CategoryId = 1 },
                new Product { Id = 6, Name = "Cheesecake", Description = "Porción de cheesecake", Price = 4.00m, Stock = 20, RestaurantId = 3, CategoryId = 3 }
            );
        }
    }
}
