using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddingTable_Gender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gendercode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderName);
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "GenderName", "Benefits", "Gendercode" },
                values: new object[,]
                {
                    { "Female", true, null },
                    { "Male", false, null },
                    { "Other", false, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}
