using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HillarysHair.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stylists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stylists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StylistId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Stylists_StylistId",
                        column: x => x.StylistId,
                        principalTable: "Stylists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentServices",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentServices", x => new { x.AppointmentId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_AppointmentServices_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "luffy@strawhat.crew", "Monkey D. Luffy", "(803) 555-0201" },
                    { 2, "zoro@strawhat.crew", "Roronoa Zoro", "(803) 555-0202" },
                    { 3, "sanji@strawhat.crew", "Vinsmoke Sanji", "(803) 555-0203" },
                    { 4, "usopp@strawhat.crew", "Usopp", "(803) 555-0204" },
                    { 5, "chopper@strawhat.crew", "Tony Tony Chopper", "(803) 555-0205" },
                    { 6, "law@heart.pirates", "Trafalgar Law", "(803) 555-0206" },
                    { 7, "ace@whitebeard.pirates", "Portgas D. Ace", "(803) 555-0207" },
                    { 8, "mihawk@shichibukai.gov", "Dracule Mihawk", "(803) 555-0208" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Haircut", 35.00m },
                    { 2, "Coloring", 75.00m },
                    { 3, "Beard Trim", 20.00m },
                    { 4, "Deep Conditioning", 45.00m }
                });

            migrationBuilder.InsertData(
                table: "Stylists",
                columns: new[] { "Id", "Email", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "nami@strawhat.crew", true, "Nami", "(803) 555-0101" },
                    { 2, "robin@strawhat.crew", true, "Nico Robin", "(803) 555-0102" },
                    { 3, "hancock@kuja.pirates", true, "Boa Hancock", "(803) 555-0103" },
                    { 4, "perona@thriller.bark", false, "Perona", "(803) 555-0104" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CustomerId", "ScheduledAt", "Status", "StylistId", "TotalCost" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Scheduled", 1, 55.00m },
                    { 2, 2, new DateTime(2026, 6, 10, 14, 0, 0, 0, DateTimeKind.Utc), "Scheduled", 2, 110.00m },
                    { 3, 3, new DateTime(2026, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), "Completed", 1, 65.00m },
                    { 4, 4, new DateTime(2026, 6, 11, 11, 0, 0, 0, DateTimeKind.Utc), "Scheduled", 3, 35.00m },
                    { 5, 5, new DateTime(2026, 6, 3, 13, 0, 0, 0, DateTimeKind.Utc), "Cancelled", 2, 110.00m },
                    { 6, 6, new DateTime(2026, 6, 12, 15, 0, 0, 0, DateTimeKind.Utc), "Scheduled", 1, 80.00m },
                    { 7, 7, new DateTime(2026, 6, 4, 10, 0, 0, 0, DateTimeKind.Utc), "Completed", 3, 20.00m },
                    { 8, 8, new DateTime(2026, 6, 13, 16, 0, 0, 0, DateTimeKind.Utc), "Scheduled", 2, 55.00m }
                });

            migrationBuilder.InsertData(
                table: "AppointmentServices",
                columns: new[] { "AppointmentId", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 3, 4 },
                    { 4, 1 },
                    { 5, 1 },
                    { 5, 2 },
                    { 6, 1 },
                    { 6, 4 },
                    { 7, 3 },
                    { 8, 1 },
                    { 8, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StylistId",
                table: "Appointments",
                column: "StylistId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_ServiceId",
                table: "AppointmentServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentServices");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Stylists");
        }
    }
}
