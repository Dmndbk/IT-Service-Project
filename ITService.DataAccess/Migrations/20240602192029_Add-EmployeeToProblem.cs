using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeToProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatedEmployeeId",
                table: "Problems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_RelatedEmployeeId",
                table: "Problems",
                column: "RelatedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Employees_RelatedEmployeeId",
                table: "Problems",
                column: "RelatedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Employees_RelatedEmployeeId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_RelatedEmployeeId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "RelatedEmployeeId",
                table: "Problems");
        }
    }
}
