using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddCountries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string SQL = "INSERT INTO Countries (CountryID, CountryName)\r\nVALUES \r\n('14629847-905a-4a0e-9abe-80b61655c5cb', 'Philippines'),\r\n('56bf46a4-02b8-4693-a0f5-0a95e2218bdc', 'Thailand'),\r\n('12e15727-d369-49a9-8b13-bc22e9362179', 'China'),\r\n('8f30bedc-47dd-4286-8950-73d8a68e5d41', 'Palestinian Territory'),\r\n('501c6d33-1bbe-45f1-8fbd-2275913c6218', 'China');\r\n";
            migrationBuilder.Sql(SQL);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
