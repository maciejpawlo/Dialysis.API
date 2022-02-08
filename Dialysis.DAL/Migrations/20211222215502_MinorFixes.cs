using Microsoft.EntityFrameworkCore.Migrations;

namespace Dialysis.DAL.Migrations
{
    public partial class MinorFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "Doctors");

            migrationBuilder.AddColumn<long>(
                name: "PermissionNumber",
                table: "Doctors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionNumber",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
