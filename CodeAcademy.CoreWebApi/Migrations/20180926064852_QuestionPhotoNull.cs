using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class QuestionPhotoNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Photos_Question_PhotoId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Photos_Question_PhotoId",
                table: "Posts",
                column: "Question_PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Photos_Question_PhotoId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Photos_Question_PhotoId",
                table: "Posts",
                column: "Question_PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
