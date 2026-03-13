using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class SettingDefaultValueToPinCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PinCode",
                table: "Persons",
                type: "int",
                nullable: true,
                defaultValue: 111111,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PinCode",
                value: 111111);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "PinCode",
                value: 111111);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "PinCode",
                value: 111111);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "PinCode",
                value: 111111);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "PinCode",
                value: 111111);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PinCode",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 111111);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PinCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "PinCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "PinCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "PinCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "PinCode",
                value: null);
        }
    }
}
