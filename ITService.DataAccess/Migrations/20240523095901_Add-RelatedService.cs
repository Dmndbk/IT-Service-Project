using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatedService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatedServiceId",
                table: "Changes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Changes_RelatedServiceId",
                table: "Changes",
                column: "RelatedServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Changes_Services_RelatedServiceId",
                table: "Changes",
                column: "RelatedServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Changes_Services_RelatedServiceId",
                table: "Changes");

            migrationBuilder.DropIndex(
                name: "IX_Changes_RelatedServiceId",
                table: "Changes");

            migrationBuilder.DropColumn(
                name: "RelatedServiceId",
                table: "Changes");
        }
    }
}
