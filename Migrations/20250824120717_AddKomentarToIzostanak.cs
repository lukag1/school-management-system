using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekatPPP.Migrations
{
    /// <inheritdoc />
    public partial class AddKomentarToIzostanak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Komentar",
                table: "Izostanci",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PredmetId",
                table: "Izostanci",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Izostanci_PredmetId",
                table: "Izostanci",
                column: "PredmetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Izostanci_Predmeti_PredmetId",
                table: "Izostanci",
                column: "PredmetId",
                principalTable: "Predmeti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Izostanci_Predmeti_PredmetId",
                table: "Izostanci");

            migrationBuilder.DropIndex(
                name: "IX_Izostanci_PredmetId",
                table: "Izostanci");

            migrationBuilder.DropColumn(
                name: "Komentar",
                table: "Izostanci");

            migrationBuilder.DropColumn(
                name: "PredmetId",
                table: "Izostanci");
        }
    }
}
