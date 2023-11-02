using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockMarketDbContextLibrary.Migrations
{
    /// <inheritdoc />
    public partial class mig_14_portfolioStock_IsTracked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTracked",
                table: "PortfolioStock",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTracked",
                table: "PortfolioStock");
        }
    }
}
