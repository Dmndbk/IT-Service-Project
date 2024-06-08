using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Assessment_AssessmentId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Clients_ClientId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClientId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessment",
                table: "Assessment");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Assessment",
                newName: "Assessments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Assessments_AssessmentId",
                table: "Requests",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Assessments_AssessmentId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments");

            migrationBuilder.RenameTable(
                name: "Assessments",
                newName: "Assessment");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessment",
                table: "Assessment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientId",
                table: "Users",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Assessment_AssessmentId",
                table: "Requests",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Clients_ClientId",
                table: "Users",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
