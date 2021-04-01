using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Additional_Start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VKLink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Campuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Criterias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AutoCheck = table.Column<bool>(type: "bit", nullable: false),
                    AutoCheckFormula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Important = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criterias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodPUDText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionSpaces", x => x.Id);
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
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faculties_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "ApplicationUserInspectionSpace",
                columns: table => new
                {
                    InspectionSpacesId = table.Column<int>(type: "int", nullable: false),
                    UserListId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserInspectionSpace", x => new { x.InspectionSpacesId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserInspectionSpace_AspNetUsers_UserListId",
                        column: x => x.UserListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserInspectionSpace_InspectionSpaces_InspectionSpacesId",
                        column: x => x.InspectionSpacesId,
                        principalTable: "InspectionSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Check",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IterationNumber = table.Column<int>(type: "int", nullable: false),
                    CurrentIteration = table.Column<int>(type: "int", nullable: false),
                    Hunt = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionSpaceId = table.Column<int>(type: "int", nullable: true),
                    InspectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Check", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Check_Check_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Check_InspectionSpaces_InspectionSpaceId",
                        column: x => x.InspectionSpaceId,
                        principalTable: "InspectionSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EduPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EduPrograms_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserCheck",
                columns: table => new
                {
                    ChecksId = table.Column<int>(type: "int", nullable: false),
                    UserListId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCheck", x => new { x.ChecksId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCheck_AspNetUsers_UserListId",
                        column: x => x.UserListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCheck_Check_ChecksId",
                        column: x => x.ChecksId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckVsCriterias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriteriaId = table.Column<int>(type: "int", nullable: true),
                    CheckId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckVsCriterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckVsCriterias_Check_CheckId",
                        column: x => x.CheckId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckVsCriterias_Criterias_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PUDs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkId = table.Column<int>(type: "int", nullable: false),
                    EduProgramId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUDs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PUDs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUDs_EduPrograms_EduProgramId",
                        column: x => x.EduProgramId,
                        principalTable: "EduPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckPUD",
                columns: table => new
                {
                    ChecksId = table.Column<int>(type: "int", nullable: false),
                    PUDListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPUD", x => new { x.ChecksId, x.PUDListId });
                    table.ForeignKey(
                        name: "FK_CheckPUD_Check_ChecksId",
                        column: x => x.ChecksId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckPUD_PUDs_PUDListId",
                        column: x => x.PUDListId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckResultEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Evaluation = table.Column<int>(type: "int", nullable: false),
                    PUDId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ValidationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckResultEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckResultEvaluations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckResultEvaluations_Check_ValidationId",
                        column: x => x.ValidationId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckResultEvaluations_PUDs_PUDId",
                        column: x => x.PUDId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Evaluation = table.Column<int>(type: "int", nullable: false),
                    PUDId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspectionCriteriaId = table.Column<int>(type: "int", nullable: true),
                    Iteration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckResults_CheckVsCriterias_InspectionCriteriaId",
                        column: x => x.InspectionCriteriaId,
                        principalTable: "CheckVsCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckResults_PUDs_PUDId",
                        column: x => x.PUDId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PUDAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Iteration = table.Column<int>(type: "int", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false),
                    InspectionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PUDId = table.Column<int>(type: "int", nullable: true),
                    ValidationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUDAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PUDAllocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUDAllocations_Check_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUDAllocations_Check_ValidationId",
                        column: x => x.ValidationId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUDAllocations_PUDs_PUDId",
                        column: x => x.PUDId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PUDChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PUDId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUDChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PUDChanges_PUDs_PUDId",
                        column: x => x.PUDId,
                        principalTable: "PUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCheck_UserListId",
                table: "ApplicationUserCheck",
                column: "UserListId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserInspectionSpace_UserListId",
                table: "ApplicationUserInspectionSpace",
                column: "UserListId");

            migrationBuilder.CreateIndex(
                name: "IX_Check_InspectionId",
                table: "Check",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Check_InspectionSpaceId",
                table: "Check",
                column: "InspectionSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPUD_PUDListId",
                table: "CheckPUD",
                column: "PUDListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResultEvaluations_PUDId",
                table: "CheckResultEvaluations",
                column: "PUDId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResultEvaluations_UserId",
                table: "CheckResultEvaluations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResultEvaluations_ValidationId",
                table: "CheckResultEvaluations",
                column: "ValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_InspectionCriteriaId",
                table: "CheckResults",
                column: "InspectionCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_PUDId",
                table: "CheckResults",
                column: "PUDId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_UserId",
                table: "CheckResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckVsCriterias_CheckId",
                table: "CheckVsCriterias",
                column: "CheckId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckVsCriterias_CriteriaId",
                table: "CheckVsCriterias",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaEmailTexts_CriteriaId",
                table: "CriteriaEmailTexts",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaEmailTexts_EmailTextId",
                table: "CriteriaEmailTexts",
                column: "EmailTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_EduPrograms_FacultyId",
                table: "EduPrograms",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_CampusId",
                table: "Faculties",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDAllocations_InspectionId",
                table: "PUDAllocations",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDAllocations_PUDId",
                table: "PUDAllocations",
                column: "PUDId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDAllocations_UserId",
                table: "PUDAllocations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDAllocations_ValidationId",
                table: "PUDAllocations",
                column: "ValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDChanges_PUDId",
                table: "PUDChanges",
                column: "PUDId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDs_DepartmentId",
                table: "PUDs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDs_EduProgramId",
                table: "PUDs",
                column: "EduProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCheck");

            migrationBuilder.DropTable(
                name: "ApplicationUserInspectionSpace");

            migrationBuilder.DropTable(
                name: "CheckPUD");

            migrationBuilder.DropTable(
                name: "CheckResultEvaluations");

            migrationBuilder.DropTable(
                name: "CheckResults");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "CriteriaEmailTexts");

            migrationBuilder.DropTable(
                name: "PUDAllocations");

            migrationBuilder.DropTable(
                name: "PUDChanges");

            migrationBuilder.DropTable(
                name: "ReportPatterns");

            migrationBuilder.DropTable(
                name: "CheckVsCriterias");

            migrationBuilder.DropTable(
                name: "EmailTexts");

            migrationBuilder.DropTable(
                name: "PUDs");

            migrationBuilder.DropTable(
                name: "Check");

            migrationBuilder.DropTable(
                name: "Criterias");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "EduPrograms");

            migrationBuilder.DropTable(
                name: "InspectionSpaces");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RealName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VKLink",
                table: "AspNetUsers");
        }
    }
}
