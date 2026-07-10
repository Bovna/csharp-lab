using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PreventDuplicateSeatBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ScreeningId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ScreeningId_SeatId",
                table: "Tickets",
                columns: new[] { "ScreeningId", "SeatId" },
                unique: true,
                filter: "[SeatId] IS NOT NULL AND [DeletedAt] IS NULL AND [Status] IN (0, 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ScreeningId_SeatId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ScreeningId",
                table: "Tickets",
                column: "ScreeningId");
        }
    }
}
