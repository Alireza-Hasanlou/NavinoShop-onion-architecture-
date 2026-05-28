using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addSlugToSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "SellerChangeRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "SellerChangeRequests");
        }
    }
}
