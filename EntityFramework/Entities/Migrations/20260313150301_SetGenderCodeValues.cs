using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class SetGenderCodeValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                    table:"Genders",
                    column:"Gendercode",
                    keyColumn:"GenderName",
                    keyValue: "Male",
                    value: "M"
                );

            migrationBuilder.UpdateData(
                    table:"Genders",
                    keyColumn:"GenderName",
                    keyValue: "Female",
                    column:"Gendercode",
                    value: "F"
                );

            migrationBuilder.UpdateData(
                    table:"Genders",
                    keyColumn:"GenderName",
                    keyValue: "Other",
                    column:"Gendercode",
                    value: "O"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
