using Microsoft.EntityFrameworkCore.Migrations;

namespace NotatkiWEB.Data.Migrations
{
    public partial class addFileModelToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoteFileCaption",
                table: "Note",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteFileURL",
                table: "Note",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteFileCaption",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "NoteFileURL",
                table: "Note");
        }
    }
}
