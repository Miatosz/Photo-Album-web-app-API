using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageAlbumAPI.Migrations
{
    public partial class th : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Users_UserID",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Albums",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Albums_UserID",
                table: "Albums",
                newName: "IX_Albums_userId");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Albums",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Users_userId",
                table: "Albums",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Users_userId",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Albums",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Albums_userId",
                table: "Albums",
                newName: "IX_Albums_UserID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Users_UserID",
                table: "Albums",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
