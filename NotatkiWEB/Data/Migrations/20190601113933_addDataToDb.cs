using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NotatkiWEB.Data.Migrations
{
    public partial class addDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SemesterList",
                columns: table => new
                {
                    IDSemester = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SemesterName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterList", x => x.IDSemester);
                });

            migrationBuilder.CreateTable(
                name: "SubjectList",
                columns: table => new
                {
                    IDSubject = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubjectName = table.Column<string>(nullable: false),
                    IDSemester = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectList", x => x.IDSubject);
                    table.ForeignKey(
                        name: "FK_SubjectList_SemesterList_IDSemester",
                        column: x => x.IDSemester,
                        principalTable: "SemesterList",
                        principalColumn: "IDSemester",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectList_IDSemester",
                table: "SubjectList",
                column: "IDSemester");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectList");

            migrationBuilder.DropTable(
                name: "SemesterList");
        }
    }
}
