using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRegistrationProgram.Migrations
{
    /// <inheritdoc />
    public partial class Changedcityclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Municipality",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "County",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Municipality",
                table: "Cities");
        }
    }
}
