using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dialysis.DAL.Migrations
{
    public partial class AddedNewTurbidityFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Turbidity",
                table: "Examinations",
                newName: "TurbidityNTU");

            migrationBuilder.AddColumn<int>(
                name: "DiastolicPressure",
                table: "Examinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SystolicPressure",
                table: "Examinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TurbidityFAU",
                table: "Examinations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiastolicPressure",
                table: "Examinations");

            migrationBuilder.DropColumn(
                name: "SystolicPressure",
                table: "Examinations");

            migrationBuilder.DropColumn(
                name: "TurbidityFAU",
                table: "Examinations");

            migrationBuilder.RenameColumn(
                name: "TurbidityNTU",
                table: "Examinations",
                newName: "Turbidity");
        }
    }
}
