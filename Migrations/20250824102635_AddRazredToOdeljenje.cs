using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekatPPP.Migrations
{
    /// <inheritdoc />
    public partial class AddRazredToOdeljenje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Razred",
                table: "Odeljenja",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Razred",
                table: "Odeljenja");
        }
    }
}
