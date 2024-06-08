using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Assets",
                type: "decimal(18,2)",
                nullable: true,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Assets");
        }
    }
}
