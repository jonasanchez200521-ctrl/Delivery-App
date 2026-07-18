using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nami.API.Migrations
{
    /// <inheritdoc />
    public partial class ExpandSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 4, "Hamburguesas, papas y combos", "Comida rápida" },
                    { 5, "Ceviches y platos de mar", "Mariscos" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "Category", "ImageUrl", "Latitude", "Longitude", "Name", "Rating", "Status" },
                values: new object[,]
                {
                    { 4, "Av. Amazonas y Naciones Unidas", "Comida rápida", "https://images.unsplash.com/photo-1571997478779-2adcbbe9ab2f?w=800&auto=format&fit=crop&q=60", -0.17849999999999999, -78.484999999999999, "Hamburguesas El Fogón", 4.4000000000000004, 0 },
                    { 5, "García Moreno N2-36, Centro Histórico", "Cafetería", "https://images.unsplash.com/photo-1554118811-1e0d58224f24?w=800&auto=format&fit=crop&q=60", -0.22009999999999999, -78.5124, "Café del Centro", 4.5999999999999996, 0 },
                    { 6, "Av. González Suárez N27-142", "Mariscos", "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?w=800&auto=format&fit=crop&q=60", -0.1963, -78.479500000000002, "Mariscos La Caleta", 4.5, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Availability", "DateBirth", "Email", "FailedLoginAttempts", "FirstName", "Gender", "LastName", "LicencePlate", "PasswordHash", "Phone", "Rating", "Status", "UserType", "VehicleType" },
                values: new object[] { 9, "Sector Carcelén", true, new DateTime(1994, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "courier3@delivery.com", 0, "Diego", 0, "Salazar", "PBX-9012", "$2a$11$8xLd3G.dMRqKGFsYHyaVp.HyA7sbThqUKlyV2aMTkGtKrP6hp1dGa", "0977777773", 4.2000000000000002, 0, "Delivery", 2 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "RestaurantId", "Stock" },
                values: new object[,]
                {
                    { 7, 4, "Carne, queso, lechuga y tomate", "https://images.unsplash.com/photo-1550547660-d9450f859349?w=500&auto=format&fit=crop&q=60", "Hamburguesa Clásica", 6.50m, 4, 40 },
                    { 8, 4, "Doble carne, tocino y salsa BBQ", "https://images.unsplash.com/photo-1571091718767-18b5b1457add?w=500&auto=format&fit=crop&q=60", "Hamburguesa BBQ", 7.50m, 4, 30 },
                    { 9, 4, "Papas fritas con queso fundido", "https://images.unsplash.com/photo-1512058564366-18510be2db19?w=500&auto=format&fit=crop&q=60", "Papas con queso", 3.50m, 4, 50 },
                    { 10, 2, "Malteada cremosa de chocolate", "https://images.unsplash.com/photo-1541167760496-1628856ab772?w=500&auto=format&fit=crop&q=60", "Malteada de chocolate", 3.00m, 4, 40 },
                    { 11, 2, "Café negro filtrado", "https://images.unsplash.com/photo-1481931098730-318b6f776db0?w=500&auto=format&fit=crop&q=60", "Café Americano", 1.75m, 5, 100 },
                    { 12, 2, "Espresso con leche vaporizada", "https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=500&auto=format&fit=crop&q=60", "Capuccino", 2.25m, 5, 80 },
                    { 13, 3, "Porción de torta húmeda de chocolate", "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=500&auto=format&fit=crop&q=60", "Torta de chocolate", 3.75m, 5, 25 },
                    { 14, 3, "Croissant de mantequilla horneado", "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=500&auto=format&fit=crop&q=60", "Croissant", 2.00m, 5, 35 },
                    { 15, 2, "Jugo de naranja natural 400ml", "https://images.unsplash.com/photo-1560781290-7dc94c0f8f4f?w=500&auto=format&fit=crop&q=60", "Jugo de naranja", 2.00m, 5, 50 },
                    { 16, 5, "Ceviche fresco de camarón con limón", "https://images.unsplash.com/photo-1554998171-89445e31c52b?w=500&auto=format&fit=crop&q=60", "Ceviche de camarón", 9.50m, 6, 25 },
                    { 17, 5, "Arroz con mariscos mixtos", "https://images.unsplash.com/photo-1548369937-47519962c11a?w=500&auto=format&fit=crop&q=60", "Arroz marinero", 8.75m, 6, 20 },
                    { 18, 5, "Sopa de pescado con yuca y cebolla curtida", "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=500&auto=format&fit=crop&q=60", "Encebollado", 6.00m, 6, 30 },
                    { 19, 2, "Limonada natural bien helada", "https://images.unsplash.com/photo-1516684732162-798a0062be99?w=500&auto=format&fit=crop&q=60", "Limonada", 1.75m, 6, 60 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
