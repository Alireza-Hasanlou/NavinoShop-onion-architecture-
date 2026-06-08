using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discount.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addShopIdToOrderDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "OrderDiscounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "OrderDiscounts");
        }
    }
}
