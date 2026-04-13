using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class NewShopMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_OrderSellers_OrderSellerId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Product_Sellers_ProductSellId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSellers_Sellers_SellerId",
                table: "OrderSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Sellers_Products_ProductId",
                table: "Product_Sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Sellers_Sellers_SellerId",
                table: "Product_Sellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product_Sellers",
                table: "Product_Sellers");

            migrationBuilder.RenameTable(
                name: "Product_Sellers",
                newName: "productSells");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Sellers_SellerId",
                table: "productSells",
                newName: "IX_productSells_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Sellers_ProductId",
                table: "productSells",
                newName: "IX_productSells_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productSells",
                table: "productSells",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_OrderSellers_OrderSellerId",
                table: "OrderItems",
                column: "OrderSellerId",
                principalTable: "OrderSellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_productSells_ProductSellId",
                table: "OrderItems",
                column: "ProductSellId",
                principalTable: "productSells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderSellers_Sellers_SellerId",
                table: "OrderSellers",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_productSells_Products_ProductId",
                table: "productSells",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_productSells_Sellers_SellerId",
                table: "productSells",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_OrderSellers_OrderSellerId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_productSells_ProductSellId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSellers_Sellers_SellerId",
                table: "OrderSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_productSells_Products_ProductId",
                table: "productSells");

            migrationBuilder.DropForeignKey(
                name: "FK_productSells_Sellers_SellerId",
                table: "productSells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productSells",
                table: "productSells");

            migrationBuilder.RenameTable(
                name: "productSells",
                newName: "Product_Sellers");

            migrationBuilder.RenameIndex(
                name: "IX_productSells_SellerId",
                table: "Product_Sellers",
                newName: "IX_Product_Sellers_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_productSells_ProductId",
                table: "Product_Sellers",
                newName: "IX_Product_Sellers_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product_Sellers",
                table: "Product_Sellers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_OrderSellers_OrderSellerId",
                table: "OrderItems",
                column: "OrderSellerId",
                principalTable: "OrderSellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Product_Sellers_ProductSellId",
                table: "OrderItems",
                column: "ProductSellId",
                principalTable: "Product_Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderSellers_Sellers_SellerId",
                table: "OrderSellers",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Sellers_Products_ProductId",
                table: "Product_Sellers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Sellers_Sellers_SellerId",
                table: "Product_Sellers",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
