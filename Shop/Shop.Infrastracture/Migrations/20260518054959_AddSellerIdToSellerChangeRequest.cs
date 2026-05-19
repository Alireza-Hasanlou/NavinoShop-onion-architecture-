using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerIdToSellerChangeRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "SellerChangeRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SellerChangeRequests_SellerId",
                table: "SellerChangeRequests",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerChangeRequests_Sellers_SellerId",
                table: "SellerChangeRequests",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerChangeRequests_Sellers_SellerId",
                table: "SellerChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_SellerChangeRequests_SellerId",
                table: "SellerChangeRequests");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "SellerChangeRequests");
        }
    }
}
