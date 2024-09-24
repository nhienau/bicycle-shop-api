using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class fixproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProductDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductDetailId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Carts",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Carts",
                newName: "ProductDetailId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_ProductDetail_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductDetailId",
                table: "ProductImage",
                column: "ProductDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProductDetail_ProductDetailId",
                table: "Carts",
                column: "ProductDetailId",
                principalTable: "ProductDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductDetail_ProductDetailId",
                table: "OrderDetails",
                column: "ProductDetailId",
                principalTable: "ProductDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductDetail_ProductDetailId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductDetail_ProductDetailId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "ProductDetailId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductDetailId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Carts",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "ProductDetailId",
                table: "Carts",
                newName: "ProductId");

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
