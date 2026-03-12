using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class FilteredPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_FilteredPersons = "CREATE PROCEDURE [dbo].[FilteredPersons]\r\n    @personProperty NVARCHAR(40),\r\n    @propertyValue NVARCHAR(100)\r\nAS\r\nBEGIN\r\n    DECLARE @sql NVARCHAR(MAX);\r\n\r\n    SET @sql = 'SELECT * FROM [dbo].[Persons] WHERE ' \r\n               + QUOTENAME(@personProperty) + ' = @propertyValue';\r\n\r\n    EXEC sp_executesql @sql, N'@propertyValue NVARCHAR(100)', @propertyValue;\r\nEND\r\n";
            migrationBuilder.Sql(sp_FilteredPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_FilteredPersons = "DROP PROCEDURE [dbo].[FilteredPersons]";
            migrationBuilder.Sql(sp_FilteredPersons);
        }
    }
}
