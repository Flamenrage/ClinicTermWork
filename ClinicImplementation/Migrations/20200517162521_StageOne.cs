using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicImplementation.Migrations
{
    public partial class StageOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrescriptionName",
                table: "TreatmentPrescriptions",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MedicationName",
                table: "RequestMedications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicationName",
                table: "MedicationPrescriptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicationName",
                table: "RequestMedications");

            migrationBuilder.DropColumn(
                name: "MedicationName",
                table: "MedicationPrescriptions");

            migrationBuilder.AlterColumn<string>(
                name: "PrescriptionName",
                table: "TreatmentPrescriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
