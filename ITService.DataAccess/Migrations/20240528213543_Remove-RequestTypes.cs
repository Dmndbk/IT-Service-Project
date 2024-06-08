using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRequestTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestTypes_RequestTypeId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestTypeId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "SolutionDescription",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainCause",
                table: "Problems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolutionDescription",
                table: "Problems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolutionDescription",
                table: "Objectives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImplementationPlan",
                table: "Changes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RollbackPlan",
                table: "Changes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolutionDescription",
                table: "Changes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SolutionDescription",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MainCause",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "SolutionDescription",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "SolutionDescription",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "ImplementationPlan",
                table: "Changes");

            migrationBuilder.DropColumn(
                name: "RollbackPlan",
                table: "Changes");

            migrationBuilder.DropColumn(
                name: "SolutionDescription",
                table: "Changes");

            migrationBuilder.AddColumn<int>(
                name: "RequestTypeId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestTypes_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId",
                principalTable: "RequestTypes",
                principalColumn: "Id");
        }
    }
}
