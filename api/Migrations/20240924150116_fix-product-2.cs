using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class fixproduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductDetail_ProductDetailId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductDetail_ProductDetailId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_Products_ProductId",
                table: "ProductDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_ProductDetail_ProductDetailId",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDetail",
                table: "ProductDetail");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "ProductImages");

            migrationBuilder.RenameTable(
                name: "ProductDetail",
                newName: "ProductDetails");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImage_ProductDetailId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetails",
                newName: "IX_ProductDetails_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProductDetails_ProductDetailId",
                table: "Carts",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductDetails_ProductDetailId",
                table: "OrderDetails",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Products_ProductId",
                table: "ProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductDetails_ProductDetailId",
                table: "ProductImages",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductDetails_ProductDetailId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductDetails_ProductDetailId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Products_ProductId",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductDetails_ProductDetailId",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImage");

            migrationBuilder.RenameTable(
                name: "ProductDetails",
                newName: "ProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductDetailId",
                table: "ProductImage",
                newName: "IX_ProductImage_ProductDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetail",
                newName: "IX_ProductDetail_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDetail",
                table: "ProductDetail",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_Products_ProductId",
                table: "ProductDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_ProductDetail_ProductDetailId",
                table: "ProductImage",
                column: "ProductDetailId",
                principalTable: "ProductDetail",
                principalColumn: "Id");
        }
    }
}
