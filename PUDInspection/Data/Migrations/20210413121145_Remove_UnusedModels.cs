using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Remove_UnusedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "CriteriaEmailTexts");

            migrationBuilder.DropTable(
                name: "ReportPatterns");

            migrationBuilder.DropTable(
                name: "EmailTexts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescriptionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoodPUDText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportPatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Formula = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPatterns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaEmailTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriteriaId = table.Column<int>(type: "int", nullable: true),
                    EmailTextId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaEmailTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriteriaEmailTexts_Criterias_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CriteriaEmailTexts_EmailTexts_EmailTextId",
                        column: x => x.EmailTextId,
                        principalTable: "EmailTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaEmailTexts_CriteriaId",
                table: "CriteriaEmailTexts",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaEmailTexts_EmailTextId",
                table: "CriteriaEmailTexts",
                column: "EmailTextId");
        }
    }
}
