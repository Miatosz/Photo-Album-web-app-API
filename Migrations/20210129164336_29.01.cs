using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageAlbumAPI.Migrations
{
    public partial class _2901 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Albums_AlbumId",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Albums_AlbumId",
                table: "Photos",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Albums_AlbumId",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Albums_AlbumId",
                table: "Photos",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
