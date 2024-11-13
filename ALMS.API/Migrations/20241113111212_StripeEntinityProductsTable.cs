using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ALMS.API.Migrations
{
    /// <inheritdoc />
    public partial class StripeEntinityProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StripeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Product = table.Column<string>(type: "TEXT", nullable: false),
                    Rate = table.Column<string>(type: "TEXT", nullable: false),
                    Quanity = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeProducts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StripeProducts");
        }
    }
}
