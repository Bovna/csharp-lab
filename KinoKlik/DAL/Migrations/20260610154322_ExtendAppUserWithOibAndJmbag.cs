using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KinoKlik.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ExtendAppUserWithOibAndJmbag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('AspNetUsers', 'JMBAG') IS NULL
                    ALTER TABLE [AspNetUsers] ADD [JMBAG] nvarchar(13) NOT NULL DEFAULT N'';
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('AspNetUsers', 'OIB') IS NULL
                    ALTER TABLE [AspNetUsers] ADD [OIB] nvarchar(11) NOT NULL DEFAULT N'';
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('AspNetUsers', 'JMBG') IS NOT NULL
                BEGIN
                    DECLARE @constraintName sysname;

                    SELECT @constraintName = dc.name
                    FROM sys.default_constraints dc
                    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
                    INNER JOIN sys.tables t ON t.object_id = c.object_id
                    WHERE t.name = 'AspNetUsers' AND c.name = 'JMBG';

                    IF @constraintName IS NOT NULL
                        EXEC('ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @constraintName + ']');

                    ALTER TABLE [AspNetUsers] DROP COLUMN [JMBG];
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('AspNetUsers', 'JMBAG') IS NOT NULL
                    ALTER TABLE [AspNetUsers] DROP COLUMN [JMBAG];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('AspNetUsers', 'OIB') IS NOT NULL
                    ALTER TABLE [AspNetUsers] DROP COLUMN [OIB];
                """);
        }
    }
}
