using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class EditPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            String sp_EditPerson = "CREATE PROCEDURE [dbo].[EditPerson](@PersonID uniqueidentifier, @PersonName nvarchar(40), @EMail nvarchar(max), @DateOfBirth datetime2(7), @Gender nvarchar(10), @Address nvarchar(60), @CountryID nvarchar(5)) AS BEGIN UPDATE Persons SET PersonName = @PersonName, Email = @Email, Gender = @Gender, Address = @Address, CountryID = @CountryID WHERE PersonID = @PersonID END";
            migrationBuilder.Sql(sp_EditPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_EditPerson = "DROP PROCEDURE [dbo].[EditPerson]";
            migrationBuilder.Sql(sp_EditPerson);
        }
    }
}
