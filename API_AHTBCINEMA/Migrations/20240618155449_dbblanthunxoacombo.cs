using Microsoft.EntityFrameworkCore.Migrations;

namespace API_AHTBCINEMA.Migrations
{
    public partial class dbblanthunxoacombo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_DoAnvaNuocs_Combo",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_Combo",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "Combo",
                table: "HoaDons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Combo",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_Combo",
                table: "HoaDons",
                column: "Combo");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_DoAnvaNuocs_Combo",
                table: "HoaDons",
                column: "Combo",
                principalTable: "DoAnvaNuocs",
                principalColumn: "IdComBo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
