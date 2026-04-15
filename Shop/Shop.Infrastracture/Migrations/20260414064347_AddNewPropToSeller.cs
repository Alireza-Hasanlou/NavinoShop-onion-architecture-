using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropToSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhyRejected",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhyRejected",
                table: "Sellers");
        }
    }
}
