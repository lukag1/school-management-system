using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekatPPP.Migrations
{
    /// <inheritdoc />
    public partial class AddOdeljenjeToUserAndIzostanci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uloga",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Izostanci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UcenikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opravdan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izostanci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Izostanci_AspNetUsers_UcenikId",
                        column: x => x.UcenikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izostanci_UcenikId",
                table: "Izostanci",
                column: "UcenikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izostanci");

            migrationBuilder.AddColumn<int>(
                name: "Uloga",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
