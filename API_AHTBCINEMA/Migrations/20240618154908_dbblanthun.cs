using Microsoft.EntityFrameworkCore.Migrations;

namespace API_AHTBCINEMA.Migrations
{
    public partial class dbblanthun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhuyenMais_KhuyenMai",
                table: "HoaDons");

            migrationBuilder.AlterColumn<int>(
                name: "KhuyenMai",
                table: "HoaDons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhuyenMais_KhuyenMai",
                table: "HoaDons",
                column: "KhuyenMai",
                principalTable: "KhuyenMais",
                principalColumn: "IdKM",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhuyenMais_KhuyenMai",
                table: "HoaDons");

            migrationBuilder.AlterColumn<int>(
                name: "KhuyenMai",
                table: "HoaDons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhuyenMais_KhuyenMai",
                table: "HoaDons",
                column: "KhuyenMai",
                principalTable: "KhuyenMais",
                principalColumn: "IdKM",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
