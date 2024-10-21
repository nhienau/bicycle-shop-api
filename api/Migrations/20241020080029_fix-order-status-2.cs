using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class fixorderstatus2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_Statusid",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "OrderStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OrderStatuses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Statusid",
                table: "Orders",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Statusid",
                table: "Orders",
                newName: "IX_Orders_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_StatusId",
                table: "Orders",
                column: "StatusId",
                principalTable: "OrderStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_StatusId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "OrderStatuses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderStatuses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Orders",
                newName: "Statusid");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                newName: "IX_Orders_Statusid");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_Statusid",
                table: "Orders",
                column: "Statusid",
                principalTable: "OrderStatuses",
                principalColumn: "id");
        }
    }
}
