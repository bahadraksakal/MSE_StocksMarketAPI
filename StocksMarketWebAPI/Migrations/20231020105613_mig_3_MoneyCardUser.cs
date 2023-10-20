using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksMarketWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_3_MoneyCardUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoneyCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<float>(type: "real", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersMoneyCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MoneyCardId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersMoneyCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersMoneyCard_MoneyCards_MoneyCardId",
                        column: x => x.MoneyCardId,
                        principalTable: "MoneyCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersMoneyCard_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersMoneyCard_MoneyCardId",
                table: "UsersMoneyCard",
                column: "MoneyCardId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersMoneyCard_UserId",
                table: "UsersMoneyCard",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersMoneyCard");

            migrationBuilder.DropTable(
                name: "MoneyCards");
        }
    }
}
