using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nami.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameCourierToDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "CourierId",
                table: "Orders",
                newName: "DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                newName: "IX_Orders_DeliveryId");

            // Cualquier cuenta real (no solo la semilla) creada como "Courier" antes de este
            // rename debe pasar a "Delivery" para seguir autenticando con el rol correcto.
            migrationBuilder.Sql(@"UPDATE ""Users"" SET ""UserType"" = 'Delivery' WHERE ""UserType"" = 'Courier';");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Availability", "DateBirth", "Email", "FirstName", "Gender", "LastName", "LicencePlate", "PasswordHash", "Phone", "Rating", "Status", "UserType", "VehicleType" },
                values: new object[,]
                {
                    { 4, "Sector La Floresta", true, new DateTime(1993, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "courier1@delivery.com", "Jorge", 0, "Ramírez", "PBX-1234", "$2a$11$8xLd3G.dMRqKGFsYHyaVp.HyA7sbThqUKlyV2aMTkGtKrP6hp1dGa", "0977777771", 4.5, 0, "Delivery", 1 },
                    { 5, "Sector El Bosque", true, new DateTime(1996, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "courier2@delivery.com", "Lucía", 1, "Fernández", "PBX-5678", "$2a$11$8xLd3G.dMRqKGFsYHyaVp.HyA7sbThqUKlyV2aMTkGtKrP6hp1dGa", "0977777772", 4.7999999999999998, 0, "Delivery", 0 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_DeliveryId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "DeliveryId",
                table: "Orders",
                newName: "CourierId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders",
                newName: "IX_Orders_CourierId");

            migrationBuilder.Sql(@"UPDATE ""Users"" SET ""UserType"" = 'Courier' WHERE ""UserType"" = 'Delivery';");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Availability", "DateBirth", "Email", "FirstName", "Gender", "LastName", "LicencePlate", "PasswordHash", "Phone", "Rating", "Status", "UserType", "VehicleType" },
                values: new object[,]
                {
                    { 4, "Sector La Floresta", true, new DateTime(1993, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "courier1@delivery.com", "Jorge", 0, "Ramírez", "PBX-1234", "$2a$11$8xLd3G.dMRqKGFsYHyaVp.HyA7sbThqUKlyV2aMTkGtKrP6hp1dGa", "0977777771", 4.5, 0, "Courier", 1 },
                    { 5, "Sector El Bosque", true, new DateTime(1996, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "courier2@delivery.com", "Lucía", 1, "Fernández", "PBX-5678", "$2a$11$8xLd3G.dMRqKGFsYHyaVp.HyA7sbThqUKlyV2aMTkGtKrP6hp1dGa", "0977777772", 4.7999999999999998, 0, "Courier", 0 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
