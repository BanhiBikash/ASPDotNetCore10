using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

    namespace Entities.Migrations
    {
        public partial class RenamePinToPinCode : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                // Rename the column from Pin → Pin Code
                migrationBuilder.RenameColumn(
                    name: "Pin",
                    table: "Persons",
                    newName: "Pin Code");

                // Alter the column definition (if you want to enforce int + default value)
                migrationBuilder.AlterColumn<int>(
                    name: "Pin Code",
                    table: "Persons",
                    type: "int",
                    nullable: true,
                    defaultValue: 111111,
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldNullable: true);
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                // Revert column definition
                migrationBuilder.AlterColumn<int>(
                    name: "Pin Code",
                    table: "Persons",
                    type: "int",
                    nullable: true,
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldNullable: true,
                    oldDefaultValue: 111111);

                // Rename back from Pin Code → Pin
                migrationBuilder.RenameColumn(
                    name: "Pin Code",
                    table: "Persons",
                    newName: "Pin");
            }
        }
    }


}
