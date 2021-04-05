using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Update_Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckPUDResult_AspNetUsers_UserId",
                table: "CheckPUDResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckPUDResult_Check_InspectionId",
                table: "CheckPUDResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckPUDResult_Check_ValidationId",
                table: "CheckPUDResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckPUDResult_CheckPUDResult_ValidationPUDResultId",
                table: "CheckPUDResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckPUDResult_PUDs_PUDId",
                table: "CheckPUDResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckResultEvaluations_CheckPUDResult_InspectionPUDResultId",
                table: "CheckResultEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_CheckPUDResult_CheckPUDResultId",
                table: "CheckResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckPUDResult",
                table: "CheckPUDResult");

            migrationBuilder.DropIndex(
                name: "IX_CheckPUDResult_InspectionId",
                table: "CheckPUDResult");

            migrationBuilder.DropIndex(
                name: "IX_CheckPUDResult_ValidationPUDResultId",
                table: "CheckPUDResult");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CheckPUDResult");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "CheckPUDResult");

            migrationBuilder.DropColumn(
                name: "ValidationPUDResultId",
                table: "CheckPUDResult");

            migrationBuilder.RenameTable(
                name: "CheckPUDResult",
                newName: "ValidationPUDResults");

            migrationBuilder.RenameColumn(
                name: "CheckPUDResultId",
                table: "CheckResults",
                newName: "ValidationPUDResultId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckResults_CheckPUDResultId",
                table: "CheckResults",
                newName: "IX_CheckResults_ValidationPUDResultId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckPUDResult_ValidationId",
                table: "ValidationPUDResults",
                newName: "IX_ValidationPUDResults_ValidationId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckPUDResult_UserId",
                table: "ValidationPUDResults",
                newName: "IX_ValidationPUDResults_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckPUDResult_PUDId",
                table: "ValidationPUDResults",
                newName: "IX_ValidationPUDResults_PUDId");

            migrationBuilder.AddColumn<string>(
                name: "InspectionPUDResultId",
                table: "CheckResults",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ValidationPUDResults",
                table: "ValidationPUDResults",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InspectionPUDResults",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InspectionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PUDId = table.Column<int>(type: "int", nullable: true),
                    ValidationPUDResultId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Iteration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionPUDResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionPUDResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionPUDResults_Check_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionPUDResults_PUDs_PUDId",
                        column: x => x.PUDId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionPUDResults_ValidationPUDResults_ValidationPUDResultId",
                        column: x => x.ValidationPUDResultId,
                        principalTable: "ValidationPUDResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_InspectionPUDResultId",
                table: "CheckResults",
                column: "InspectionPUDResultId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPUDResults_InspectionId",
                table: "InspectionPUDResults",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPUDResults_PUDId",
                table: "InspectionPUDResults",
                column: "PUDId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPUDResults_UserId",
                table: "InspectionPUDResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPUDResults_ValidationPUDResultId",
                table: "InspectionPUDResults",
                column: "ValidationPUDResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResultEvaluations_InspectionPUDResults_InspectionPUDResultId",
                table: "CheckResultEvaluations",
                column: "InspectionPUDResultId",
                principalTable: "InspectionPUDResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResults_InspectionPUDResults_InspectionPUDResultId",
                table: "CheckResults",
                column: "InspectionPUDResultId",
                principalTable: "InspectionPUDResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResults_ValidationPUDResults_ValidationPUDResultId",
                table: "CheckResults",
                column: "ValidationPUDResultId",
                principalTable: "ValidationPUDResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationPUDResults_AspNetUsers_UserId",
                table: "ValidationPUDResults",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationPUDResults_Check_ValidationId",
                table: "ValidationPUDResults",
                column: "ValidationId",
                principalTable: "Check",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationPUDResults_PUDs_PUDId",
                table: "ValidationPUDResults",
                column: "PUDId",
                principalTable: "PUDs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResultEvaluations_InspectionPUDResults_InspectionPUDResultId",
                table: "CheckResultEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_InspectionPUDResults_InspectionPUDResultId",
                table: "CheckResults");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_ValidationPUDResults_ValidationPUDResultId",
                table: "CheckResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationPUDResults_AspNetUsers_UserId",
                table: "ValidationPUDResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationPUDResults_Check_ValidationId",
                table: "ValidationPUDResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationPUDResults_PUDs_PUDId",
                table: "ValidationPUDResults");

            migrationBuilder.DropTable(
                name: "InspectionPUDResults");

            migrationBuilder.DropIndex(
                name: "IX_CheckResults_InspectionPUDResultId",
                table: "CheckResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ValidationPUDResults",
                table: "ValidationPUDResults");

            migrationBuilder.DropColumn(
                name: "InspectionPUDResultId",
                table: "CheckResults");

            migrationBuilder.RenameTable(
                name: "ValidationPUDResults",
                newName: "CheckPUDResult");

            migrationBuilder.RenameColumn(
                name: "ValidationPUDResultId",
                table: "CheckResults",
                newName: "CheckPUDResultId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckResults_ValidationPUDResultId",
                table: "CheckResults",
                newName: "IX_CheckResults_CheckPUDResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ValidationPUDResults_ValidationId",
                table: "CheckPUDResult",
                newName: "IX_CheckPUDResult_ValidationId");

            migrationBuilder.RenameIndex(
                name: "IX_ValidationPUDResults_UserId",
                table: "CheckPUDResult",
                newName: "IX_CheckPUDResult_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ValidationPUDResults_PUDId",
                table: "CheckPUDResult",
                newName: "IX_CheckPUDResult_PUDId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CheckPUDResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InspectionId",
                table: "CheckPUDResult",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationPUDResultId",
                table: "CheckPUDResult",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckPUDResult",
                table: "CheckPUDResult",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_InspectionId",
                table: "CheckPUDResult",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUDResult_ValidationPUDResultId",
                table: "CheckPUDResult",
                column: "ValidationPUDResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckPUDResult_AspNetUsers_UserId",
                table: "CheckPUDResult",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckPUDResult_Check_InspectionId",
                table: "CheckPUDResult",
                column: "InspectionId",
                principalTable: "Check",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckPUDResult_Check_ValidationId",
                table: "CheckPUDResult",
                column: "ValidationId",
                principalTable: "Check",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckPUDResult_CheckPUDResult_ValidationPUDResultId",
                table: "CheckPUDResult",
                column: "ValidationPUDResultId",
                principalTable: "CheckPUDResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckPUDResult_PUDs_PUDId",
                table: "CheckPUDResult",
                column: "PUDId",
                principalTable: "PUDs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
