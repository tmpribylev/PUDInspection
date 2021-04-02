using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Update_CheckingPUDStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResultEvaluations_PUDs_PUDId",
                table: "CheckResultEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_AspNetUsers_UserId",
                table: "CheckResults");

            migrationBuilder.DropIndex(
                name: "IX_CheckResultEvaluations_PUDId",
                table: "CheckResultEvaluations");

            migrationBuilder.DropColumn(
                name: "Iteration",
                table: "CheckResults");

            migrationBuilder.DropColumn(
                name: "PUDId",
                table: "CheckResultEvaluations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CheckResults",
                newName: "CheckPUDResultId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckResults_UserId",
                table: "CheckResults",
                newName: "IX_CheckResults_CheckPUDResultId");

            migrationBuilder.AddColumn<string>(
                name: "InspectionPUDResultId",
                table: "CheckResultEvaluations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CheckPUDResult",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PUDId = table.Column<int>(type: "int", nullable: true),
                    Iteration = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionId = table.Column<int>(type: "int", nullable: true),
                    ValidationPUDResultId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ValidationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPUDResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPUDResult_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPUDResult_Check_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPUDResult_Check_ValidationId",
                        column: x => x.ValidationId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPUDResult_CheckPUDResult_ValidationPUDResultId",
                        column: x => x.ValidationPUDResultId,
                        principalTable: "CheckPUDResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPUDResult_PUDs_PUDId",
                        column: x => x.PUDId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckResultEvaluations_InspectionPUDResultId",
                table: "CheckResultEvaluations",
                column: "InspectionPUDResultId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_InspectionId",
                table: "CheckPUDResult",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_PUDId",
                table: "CheckPUDResult",
                column: "PUDId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_UserId",
                table: "CheckPUDResult",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_ValidationId",
                table: "CheckPUDResult",
                column: "ValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_ValidationPUDResultId",
                table: "CheckPUDResult",
                column: "ValidationPUDResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResultEvaluations_CheckPUDResult_InspectionPUDResultId",
                table: "CheckResultEvaluations",
                column: "InspectionPUDResultId",
                principalTable: "CheckPUDResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResults_CheckPUDResult_CheckPUDResultId",
                table: "CheckResults",
                column: "CheckPUDResultId",
                principalTable: "CheckPUDResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResultEvaluations_CheckPUDResult_InspectionPUDResultId",
                table: "CheckResultEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_CheckPUDResult_CheckPUDResultId",
                table: "CheckResults");

            migrationBuilder.DropTable(
                name: "CheckPUDResult");

            migrationBuilder.DropIndex(
                name: "IX_CheckResultEvaluations_InspectionPUDResultId",
                table: "CheckResultEvaluations");

            migrationBuilder.DropColumn(
                name: "InspectionPUDResultId",
                table: "CheckResultEvaluations");

            migrationBuilder.RenameColumn(
                name: "CheckPUDResultId",
                table: "CheckResults",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckResults_CheckPUDResultId",
                table: "CheckResults",
                newName: "IX_CheckResults_UserId");

            migrationBuilder.AddColumn<int>(
                name: "Iteration",
                table: "CheckResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PUDId",
                table: "CheckResultEvaluations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckResultEvaluations_PUDId",
                table: "CheckResultEvaluations",
                column: "PUDId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResultEvaluations_PUDs_PUDId",
                table: "CheckResultEvaluations",
                column: "PUDId",
                principalTable: "PUDs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResults_AspNetUsers_UserId",
                table: "CheckResults",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
