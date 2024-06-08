using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStatusId",
                table: "Problems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStatusId",
                table: "Objectives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStatusId",
                table: "Changes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mark = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssessmentId",
                table: "Requests",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_CurrentStatusId",
                table: "Problems",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_CurrentStatusId",
                table: "Objectives",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Changes_CurrentStatusId",
                table: "Changes",
                column: "CurrentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Changes_Status_CurrentStatusId",
                table: "Changes",
                column: "CurrentStatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Status_CurrentStatusId",
                table: "Objectives",
                column: "CurrentStatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Status_CurrentStatusId",
                table: "Problems",
                column: "CurrentStatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Assessment_AssessmentId",
                table: "Requests",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Changes_Status_CurrentStatusId",
                table: "Changes");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Status_CurrentStatusId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Status_CurrentStatusId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Assessment_AssessmentId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AssessmentId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Problems_CurrentStatusId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_CurrentStatusId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_Changes_CurrentStatusId",
                table: "Changes");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrentStatusId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "CurrentStatusId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "CurrentStatusId",
                table: "Changes");
        }
    }
}
