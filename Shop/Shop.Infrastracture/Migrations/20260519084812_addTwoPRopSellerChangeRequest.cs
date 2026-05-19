using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addTwoPRopSellerChangeRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SellerChangeRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhyRejected",
                table: "SellerChangeRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SellerChangeRequests");

            migrationBuilder.DropColumn(
                name: "WhyRejected",
                table: "SellerChangeRequests");
        }
    }
}
