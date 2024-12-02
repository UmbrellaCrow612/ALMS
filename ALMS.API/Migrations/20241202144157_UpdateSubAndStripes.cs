using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ALMS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubAndStripes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeProductId",
                table: "Subscriptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "StripeSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StripeProductId",
                table: "Subscriptions",
                column: "StripeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StripeSessions_ProductId",
                table: "StripeSessions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StripeSessions_StripeProducts_ProductId",
                table: "StripeSessions",
                column: "ProductId",
                principalTable: "StripeProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_StripeProducts_StripeProductId",
                table: "Subscriptions",
                column: "StripeProductId",
                principalTable: "StripeProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StripeSessions_StripeProducts_ProductId",
                table: "StripeSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_StripeProducts_StripeProductId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_StripeProductId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_StripeSessions_ProductId",
                table: "StripeSessions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StripeProductId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "StripeSessions");
        }
    }
}
