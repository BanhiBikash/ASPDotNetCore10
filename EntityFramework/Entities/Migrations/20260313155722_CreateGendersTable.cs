using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class CreateGendersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GenderKey = table.Column<int>(type: "int", nullable: false),
                    Benefits = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderKey);
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "GenderName", "GenderKey", "Benefits" },
                values: new object[,]
                {
                    { "Female", 11, null },
                    { "Male", 21, null },
                    { "Other", 31, null }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
