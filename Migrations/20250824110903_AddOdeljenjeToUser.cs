using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekatPPP.Migrations
{
    /// <inheritdoc />
    public partial class AddOdeljenjeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NastavnikPredmeti",
                newName: "OdeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_NastavnikPredmeti_OdeljenjeId",
                table: "NastavnikPredmeti",
                column: "OdeljenjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NastavnikPredmeti_AspNetUsers_NastavnikId",
                table: "NastavnikPredmeti",
                column: "NastavnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NastavnikPredmeti_Odeljenja_OdeljenjeId",
                table: "NastavnikPredmeti",
                column: "OdeljenjeId",
                principalTable: "Odeljenja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NastavnikPredmeti_AspNetUsers_NastavnikId",
                table: "NastavnikPredmeti");

            migrationBuilder.DropForeignKey(
                name: "FK_NastavnikPredmeti_Odeljenja_OdeljenjeId",
                table: "NastavnikPredmeti");

            migrationBuilder.DropIndex(
                name: "IX_NastavnikPredmeti_OdeljenjeId",
                table: "NastavnikPredmeti");

            migrationBuilder.RenameColumn(
                name: "OdeljenjeId",
                table: "NastavnikPredmeti",
                newName: "Id");
        }
    }
}
