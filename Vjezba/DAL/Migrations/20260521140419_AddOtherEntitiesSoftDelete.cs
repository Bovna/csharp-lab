using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherEntitiesSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Seats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Halls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CustomerFavoriteMovies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Cinemas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 2,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 3,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 4,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 5,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 6,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 7,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CustomerFavoriteMovies",
                keyColumn: "Id",
                keyValue: 8,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 6,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 7,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 8,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 201611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 202609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 203709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204710,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 204711,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 205609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 301611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 302611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 303709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304710,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 304711,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 305609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306710,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306711,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306801,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306802,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306803,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306804,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306805,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306806,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306807,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306808,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306809,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306810,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 306811,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 401611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402710,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402711,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402801,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402802,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402803,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402804,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402805,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402806,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402807,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402808,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402809,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402810,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 402811,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 403709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 501611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 502609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 503709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504710,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 504711,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505101,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505102,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505103,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505104,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505105,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505106,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505107,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505108,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505109,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505110,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505111,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505201,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505202,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505203,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505204,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505205,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505206,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505207,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505208,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505209,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505210,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505211,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505301,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505302,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505303,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505304,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505305,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505306,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505307,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505308,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505309,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505310,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505311,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505401,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505402,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505403,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505404,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505405,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505406,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505407,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505408,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505409,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505410,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505411,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505501,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505502,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505503,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505504,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505505,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505506,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505507,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505508,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505509,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505510,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505511,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505601,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505602,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505603,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505604,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505605,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505606,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505607,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505608,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505609,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505610,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505611,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505701,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505702,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505703,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505704,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505705,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505706,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505707,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505708,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505709,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505710,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 505711,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 6,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 7,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 8,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 9,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 10,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 11,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 12,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 13,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 14,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 15,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 16,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 17,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 18,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 19,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 20,
                column: "DeletedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Halls");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CustomerFavoriteMovies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Cinemas");
        }
    }
}
