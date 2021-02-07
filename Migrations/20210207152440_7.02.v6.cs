using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageAlbumAPI.Migrations
{
    public partial class _702v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
