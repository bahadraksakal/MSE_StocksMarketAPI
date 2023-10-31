using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangFireDbContextLibrary.Migrations
{
    /// <inheritdoc />
    public partial class mig_1_stock_hang_fire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockHangFire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHangFire", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockHangFire_Name",
                table: "StockHangFire",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockHangFire");
        }
    }
}
