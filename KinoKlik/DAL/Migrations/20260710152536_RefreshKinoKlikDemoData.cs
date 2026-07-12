using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KinoKlik.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefreshKinoKlikDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "zagreb@kinoklik.example", "12", "KinoKlik Zagreb", "+385 1 555 0101", "Filmska ulica" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "rijeka@kinoklik.example", "8", "KinoKlik Rijeka", "+385 51 555 0202", "Svjetionikova" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "osijek@kinoklik.example", "17", "KinoKlik Osijek", "+385 31 555 0303", "Platnena avenija" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "split@kinoklik.example", "4", "KinoKlik Split", "+385 21 555 0404", "Kadrova obala" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "zadar@kinoklik.example", "9", "KinoKlik Zadar", "+385 23 555 0505", "Morski kadar" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "branimir@cinestar.hr", "29", "CineStar Branimir", "+385 1 111 222", "Branimirova" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "info@kinoeuropa.hr", "14", "Kino Europa", "+385 51 333 444", "Korzo" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "kontakt@arenacinema.hr", "6", "Arena Cinema", "+385 31 555 666", "Sjenjak" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "hello@marinacineplex.hr", "9", "Marina Cineplex", "+385 21 777 888", "Obala" });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "HouseNumber", "Name", "Phone", "Street" },
                values: new object[] { "info@forumcinema.hr", "12", "Forum Cinema", "+385 23 456 700", "Siroka" });
        }
    }
}
