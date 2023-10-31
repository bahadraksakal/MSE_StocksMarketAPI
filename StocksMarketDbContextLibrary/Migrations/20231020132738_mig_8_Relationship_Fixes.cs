using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockMarketDbContextLibrary.Migrations
{
    /// <inheritdoc />
    public partial class mig_8_Relationship_Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersMoneyCard_MoneyCardId",
                table: "UsersMoneyCard");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioUser_PortfolioId",
                table: "PortfolioUser");

            migrationBuilder.CreateIndex(
                name: "IX_UsersMoneyCard_MoneyCardId",
                table: "UsersMoneyCard",
                column: "MoneyCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioUser_PortfolioId",
                table: "PortfolioUser",
                column: "PortfolioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersMoneyCard_MoneyCardId",
                table: "UsersMoneyCard");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioUser_PortfolioId",
                table: "PortfolioUser");

            migrationBuilder.CreateIndex(
                name: "IX_UsersMoneyCard_MoneyCardId",
                table: "UsersMoneyCard",
                column: "MoneyCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioUser_PortfolioId",
                table: "PortfolioUser",
                column: "PortfolioId");
        }
    }
}
