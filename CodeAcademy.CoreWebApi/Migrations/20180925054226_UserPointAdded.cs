using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class UserPointAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "AspNetUsers");
        }
    }
}
