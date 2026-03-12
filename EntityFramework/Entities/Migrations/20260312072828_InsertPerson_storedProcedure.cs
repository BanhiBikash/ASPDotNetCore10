using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_storedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = "CREATE PROCEDURE [dbo].[InsertPerson](@PersonID uniqueidentifier, @PersonName nvarchar(40), @EMail nvarchar(max), @DateOfBirth datetime2(7), @Gender nvarchar(10), @Address nvarchar(60), @CountryID nvarchar(5)) AS BEGIN INSERT INTO [dbo].[Persons] (PersonID, PersonName, Email, DateOfBirth, Gender, Address, CountryID) Values (@PersonID, @PersonName, @EMail, @DateOfBirth, @Gender, @Address, @CountryID) END";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = "DROP PROCEDURE [dbo].[InsertPerson]";
            migrationBuilder.Sql(sp_InsertPerson);  
        }
    }
}
