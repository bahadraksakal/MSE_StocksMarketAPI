using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksMarketWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_11_SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainBoards",
                columns: new[] { "Id", "Balance", "CommissionRate" },
                values: new object[] { 5, 0f, 5f });

            migrationBuilder.InsertData(
                table: "Portfolios",
                columns: new[] { "Id", "Balance" },
                values: new object[] { 1, 0f });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role", "Tel" },
                values: new object[] { 1, "admin@gmail.com", "admin", "admin", "Admin", "05350449876" });

            migrationBuilder.InsertData(
                table: "PortfolioUser",
                columns: new[] { "Id", "PortfolioId", "UserId" },
                values: new object[] { 1, 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainBoards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PortfolioUser",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Portfolios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
