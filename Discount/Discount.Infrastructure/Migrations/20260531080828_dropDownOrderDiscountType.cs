using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discount.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dropDownOrderDiscountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDiscountType",
                table: "ProductDiscounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDiscountType",
                table: "ProductDiscounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
