using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class SortPersonsByProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sortPersonsByProperty =
            @" CREATE PROCEDURE [dbo].[SortPersonsByProperty] @propertyName NVARCHAR(50)
            AS BEGIN
            DECLARE @sql NVARCHAR(MAX);

            SET @sql = 'SELECT * FROM [dbo].[Persons] ORDER BY ' + QUOTENAME(@propertyName);

            EXEC sp_executesql @sql;
            END";

            migrationBuilder.Sql(sortPersonsByProperty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sortPersonsByProperty = @" DROP PROCEDURE [dbo].[SortPersonsByProperty]";

            migrationBuilder.Sql(sortPersonsByProperty);
        }
    }
}
