using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    CountryID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "123 Maple Street, New York", "US", new DateTime(1990, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice.johnson@example.com", "Female", "Alice Johnson" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "456 Oak Avenue, Chicago", "US", new DateTime(1985, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob.smith@example.com", "Male", "Bob Smith" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "789 Pine Road, San Francisco", "US", new DateTime(1992, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "catherine.lee@example.com", "Female", "Catherine Lee" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "12 MG Road, Bengaluru", "IN", new DateTime(1988, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "david.kumar@example.com", "Male", "David Kumar" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "34 Queen Street, London", "UK", new DateTime(1995, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma.brown@example.com", "Female", "Emma Brown" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
