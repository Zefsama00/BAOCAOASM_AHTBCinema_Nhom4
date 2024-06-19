using Microsoft.EntityFrameworkCore.Migrations;

namespace API_AHTBCINEMA.Migrations
{
    public partial class dbmoilan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "NhanViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "KhachHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "GioChieus",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "NhanViens");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "GioChieus");
        }
    }
}
