using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefreshScreeningSeedDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 10, 20, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 18, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 10, 22, 50, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 21, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 11, 18, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 16, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 11, 1, 5, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 22, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1005,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 11, 21, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 19, 20, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 11, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 17, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 11, 21, 40, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 12, 20, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 19, 10, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 12, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 11, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 12, 22, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 12, 22, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 21, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 13, 20, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 13, 18, 50, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 14, 1, 25, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 13, 23, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 13, 20, 22, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 13, 18, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 13, 22, 58, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 13, 21, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 14, 21, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 14, 19, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 14, 18, 7, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 14, 16, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4005,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 15, 22, 38, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 20, 40, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 14, 22, 13, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 14, 20, 10, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 15, 21, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 19, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 16, 19, 21, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 16, 17, 45, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 16, 22, 9, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 16, 20, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5005,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 17, 20, 5, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 17, 18, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5006,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 16, 0, 28, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 22, 25, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5007,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 9, 16, 15, 48, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 16, 14, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 18, 20, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 18, 18, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 18, 22, 50, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 18, 21, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 19, 18, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 19, 16, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 5, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 18, 22, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 1005,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 19, 21, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 19, 19, 20, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 19, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 19, 17, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 19, 21, 40, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 19, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 20, 20, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 20, 19, 10, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 2004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 20, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 20, 11, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 20, 22, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 20, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 20, 22, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 20, 21, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 21, 18, 50, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 3004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 22, 1, 25, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 21, 23, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 22, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 21, 18, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 21, 22, 58, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 21, 21, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 22, 21, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 22, 19, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 22, 18, 7, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 22, 16, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 4005,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 23, 22, 38, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 23, 20, 40, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5001,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 22, 22, 13, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 22, 20, 10, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5002,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 23, 21, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 23, 19, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5003,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 24, 19, 21, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 24, 17, 45, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5004,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 24, 22, 9, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 24, 20, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5005,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 25, 20, 5, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 25, 18, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5006,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 24, 0, 28, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 23, 22, 25, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: 5007,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2026, 4, 24, 15, 48, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 24, 14, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
