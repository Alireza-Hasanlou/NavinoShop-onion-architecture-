using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financial.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeApropName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Transactions",
                newName: "TransationById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransationById",
                table: "Transactions",
                newName: "OwnerId");
        }
    }
}
