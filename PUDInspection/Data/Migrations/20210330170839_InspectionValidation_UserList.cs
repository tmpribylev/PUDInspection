using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class InspectionValidation_UserList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Check_CheckId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Check_AspNetUsers_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropForeignKey(
                name: "FK_Check_AspNetUsers_Validation_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_Check_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_Check_Validation_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CheckId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Check");

            migrationBuilder.DropColumn(
                name: "Validation_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropColumn(
                name: "CheckId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserInspection",
                columns: table => new
                {
                    InspectionsId = table.Column<int>(type: "int", nullable: false),
                    UserListId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserInspection", x => new { x.InspectionsId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserInspection_AspNetUsers_UserListId",
                        column: x => x.UserListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserInspection_Check_InspectionsId",
                        column: x => x.InspectionsId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserValidation",
                columns: table => new
                {
                    UserListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValidationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserValidation", x => new { x.UserListId, x.ValidationsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserValidation_AspNetUsers_UserListId",
                        column: x => x.UserListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserValidation_Check_ValidationsId",
                        column: x => x.ValidationsId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserInspection_UserListId",
                table: "ApplicationUserInspection",
                column: "UserListId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserValidation_ValidationsId",
                table: "ApplicationUserValidation",
                column: "ValidationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserInspection");

            migrationBuilder.DropTable(
                name: "ApplicationUserValidation");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Check",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Validation_ApplicationUserId",
                table: "Check",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheckId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Check_ApplicationUserId",
                table: "Check",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Check_Validation_ApplicationUserId",
                table: "Check",
                column: "Validation_ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CheckId",
                table: "AspNetUsers",
                column: "CheckId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Check_CheckId",
                table: "AspNetUsers",
                column: "CheckId",
                principalTable: "Check",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Check_AspNetUsers_ApplicationUserId",
                table: "Check",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Check_AspNetUsers_Validation_ApplicationUserId",
                table: "Check",
                column: "Validation_ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
