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
                },
                new Delivery
                {
                    // Id 6, 7 y 8 quedaron ocupados por datos creados manualmente durante pruebas
                    // end-to-end previas (verify.e2e@test.com, nuevo.repartidor@test.com y un registro
                    // sin correo); se usa el siguiente Id libre para no chocar con esos registros reales.
                    Id = 9,
                    FirstName = "Diego",
                    LastName = "Salazar",
                    Address = "Sector Carcelén",
                    Email = "courier3@delivery.com",
                    Phone = "0977777773",
                    Gender = Gender.Male,
                    PasswordHash = DeliveryPasswordHash,
                    DateBirth = new DateTime(1994, 7, 22, 0, 0, 0, DateTimeKind.Utc),
                    Status = UserStatus.Active,
                    VehicleType = VehicleType.Car,
                    LicencePlate = "PBX-9012",
                    Rating = 4.2,
                    Availability = true
                });

            // Coordenadas reales de Quito: cada restaurante en una zona distinta de la ciudad.
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1, Name = "La Parrilla Criolla", Address = "Calle 10 y 5", Category = "Comida típica",
                    Status = RestaurantStatus.Active, Rating = 4.6,
                    ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=800&auto=format&fit=crop&q=60",
                    Latitude = -0.1953, Longitude = -78.4854 // La Mariscal
                },
                new Restaurant
                {
                    Id = 2, Name = "Pizza Nostra", Address = "Av. Amazonas 234", Category = "Pizzería",
                    Status = RestaurantStatus.Active, Rating = 4.3,
                    ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ca4b?w=800&auto=format&fit=crop&q=60",
                    Latitude = -0.2082, Longitude = -78.4923 // La Floresta
                },
                new Restaurant
                {
                    Id = 3, Name = "Sushi Zen", Address = "Av. 6 de Diciembre 500", Category = "Comida japonesa",
                    Status = RestaurantStatus.Active, Rating = 4.7,
                    ImageUrl = "https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=800&auto=format&fit=crop&q=60",
                    Latitude = -0.1998, Longitude = -78.4327 // Cumbayá
                },
                new Restaurant
                {
                    Id = 4, Name = "Hamburguesas El Fogón", Address = "Av. Amazonas y Naciones Unidas", Category = "Comida rápida",
                    Status = RestaurantStatus.Active, Rating = 4.4,
                    ImageUrl = "https://images.unsplash.com/photo-1571997478779-2adcbbe9ab2f?w=800&auto=format&fit=crop&q=60",
                    Latitude = -0.1785, Longitude = -78.4850 // La Carolina
                },
                new Restaurant
                {
                    Id = 5, Name = "Café del Centro", Address = "García Moreno N2-36, Centro Histórico", Category = "Cafetería",
                    Status = RestaurantStatus.Active, Rating = 4.6,
                    ImageUrl = "https://images.unsplash.com/photo-1554118811-1e0d58224f24?w=800&auto=format&fit=crop&q=60",
                    Latitude = -0.2201, Longitude = -78.5124 // Centro Histórico
                },
                new Restaurant
                {
                    Id = 6, Name = "Mariscos La Caleta", Address = "Av. González Suárez N27-142", Category = "Mariscos",
                    Status = RestaurantStatus.Active, Rating = 4.5,
                    ImageUrl = "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?w=800&auto=format&fit=crop&q=60",
                    Latitude = -0.1963, Longitude = -78.4795 // González Suárez
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Platos fuertes", Description = "Platos principales del menú" },
                new Category { Id = 2, Name = "Bebidas", Description = "Bebidas frías y calientes" },
                new Category { Id = 3, Name = "Postres", Description = "Postres y dulces" },
                new Category { Id = 4, Name = "Comida rápida", Description = "Hamburguesas, papas y combos" },
                new Category { Id = 5, Name = "Mariscos", Description = "Ceviches y platos de mar" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1, Name = "Churrasco Mixto", Description = "Carne, huevo, plátano y arroz", Price = 8.50m, Stock = 50,
                    RestaurantId = 1, CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1600891964092-4316c288032e?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 2, Name = "Jugo Natural", Description = "Jugo de fruta natural 500ml", Price = 2.00m, Stock = 100,
                    RestaurantId = 1, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1600271886742-f049cd451bba?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 3, Name = "Pizza Margarita", Description = "Pizza clásica de mozzarella y albahaca", Price = 9.00m, Stock = 40,
                    RestaurantId = 2, CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 4, Name = "Gaseosa 500ml", Description = "Bebida gaseosa", Price = 1.50m, Stock = 100,
                    RestaurantId = 2, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1554866585-cd94860890b7?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 5, Name = "Sushi Roll California", Description = "8 piezas de roll california", Price = 7.50m, Stock = 30,
                    RestaurantId = 3, CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1553621042-f6e147245754?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 6, Name = "Cheesecake", Description = "Porción de cheesecake", Price = 4.00m, Stock = 20,
                    RestaurantId = 3, CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1533134242443-d4fd215305ad?w=500&auto=format&fit=crop&q=60"
                },

                // Hamburguesas El Fogón (RestaurantId = 4)
                new Product
                {
                    Id = 7, Name = "Hamburguesa Clásica", Description = "Carne, queso, lechuga y tomate", Price = 6.50m, Stock = 40,
                    RestaurantId = 4, CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1550547660-d9450f859349?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 8, Name = "Hamburguesa BBQ", Description = "Doble carne, tocino y salsa BBQ", Price = 7.50m, Stock = 30,
                    RestaurantId = 4, CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1571091718767-18b5b1457add?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 9, Name = "Papas con queso", Description = "Papas fritas con queso fundido", Price = 3.50m, Stock = 50,
                    RestaurantId = 4, CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1512058564366-18510be2db19?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 10, Name = "Malteada de chocolate", Description = "Malteada cremosa de chocolate", Price = 3.00m, Stock = 40,
                    RestaurantId = 4, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1541167760496-1628856ab772?w=500&auto=format&fit=crop&q=60"
                },

                // Café del Centro (RestaurantId = 5)
                new Product
                {
                    Id = 11, Name = "Café Americano", Description = "Café negro filtrado", Price = 1.75m, Stock = 100,
                    RestaurantId = 5, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1481931098730-318b6f776db0?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 12, Name = "Capuccino", Description = "Espresso con leche vaporizada", Price = 2.25m, Stock = 80,
                    RestaurantId = 5, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 13, Name = "Torta de chocolate", Description = "Porción de torta húmeda de chocolate", Price = 3.75m, Stock = 25,
                    RestaurantId = 5, CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 14, Name = "Croissant", Description = "Croissant de mantequilla horneado", Price = 2.00m, Stock = 35,
                    RestaurantId = 5, CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 15, Name = "Jugo de naranja", Description = "Jugo de naranja natural 400ml", Price = 2.00m, Stock = 50,
                    RestaurantId = 5, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1560781290-7dc94c0f8f4f?w=500&auto=format&fit=crop&q=60"
                },

                // Mariscos La Caleta (RestaurantId = 6)
                new Product
                {
                    Id = 16, Name = "Ceviche de camarón", Description = "Ceviche fresco de camarón con limón", Price = 9.50m, Stock = 25,
                    RestaurantId = 6, CategoryId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1554998171-89445e31c52b?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 17, Name = "Arroz marinero", Description = "Arroz con mariscos mixtos", Price = 8.75m, Stock = 20,
                    RestaurantId = 6, CategoryId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1548369937-47519962c11a?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 18, Name = "Encebollado", Description = "Sopa de pescado con yuca y cebolla curtida", Price = 6.00m, Stock = 30,
                    RestaurantId = 6, CategoryId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=500&auto=format&fit=crop&q=60"
                },
                new Product
                {
                    Id = 19, Name = "Limonada", Description = "Limonada natural bien helada", Price = 1.75m, Stock = 60,
                    RestaurantId = 6, CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1516684732162-798a0062be99?w=500&auto=format&fit=crop&q=60"
                }
            );
        }
    }
}
