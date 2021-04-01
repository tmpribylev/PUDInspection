using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Add_CheckStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Started",
                table: "Check",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Started",
                table: "Check");
        }
    }
}
