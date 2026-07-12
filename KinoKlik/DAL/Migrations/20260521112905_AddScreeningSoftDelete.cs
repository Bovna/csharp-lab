using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KinoKlik.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddScreeningSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Screenings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1001,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1002,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1003,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1004,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1005,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2001,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2002,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2003,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2004,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3001,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3002,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3003,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3004,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4001,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4002,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4003,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4004,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4005,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5001,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5002,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5003,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5004,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5005,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5006,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5007,
                column: "DeletedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Screenings");
        }
    }
}
