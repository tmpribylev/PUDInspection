using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Remove_UserBackpack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inspectPUDCriteriaViewModels");

            migrationBuilder.DropTable(
                name: "inspectPUDViewModels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inspectPUDViewModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AllocationId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentIteration = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EduProgram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PUDId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inspectPUDViewModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inspectPUDViewModels_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inspectPUDCriteriaViewModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckResult = table.Column<int>(type: "int", nullable: false),
                    CheckVsCriteriaId = table.Column<int>(type: "int", nullable: false),
                    CriteriaId = table.Column<int>(type: "int", nullable: true),
                    InspectPUDViewModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inspectPUDCriteriaViewModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inspectPUDCriteriaViewModels_Criterias_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inspectPUDCriteriaViewModels_inspectPUDViewModels_InspectPUDViewModelId",
                        column: x => x.InspectPUDViewModelId,
                        principalTable: "inspectPUDViewModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inspectPUDCriteriaViewModels_CriteriaId",
                table: "inspectPUDCriteriaViewModels",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_inspectPUDCriteriaViewModels_InspectPUDViewModelId",
                table: "inspectPUDCriteriaViewModels",
                column: "InspectPUDViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_inspectPUDViewModels_ApplicationUserId",
                table: "inspectPUDViewModels",
                column: "ApplicationUserId");
        }
    }
}
