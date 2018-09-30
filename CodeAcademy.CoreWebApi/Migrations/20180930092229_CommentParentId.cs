using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class CommentParentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Comments");
        }
    }
}
