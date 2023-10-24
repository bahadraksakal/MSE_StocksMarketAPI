using Microsoft.EntityFrameworkCore;
using StocksMarketWebAPI.Entities;
using System;

namespace StocksMarketWebAPI.Context
{
    public class StockMarketDbContext :DbContext
    {
        public virtual DbSet<MainBoard> MainBoards { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<PortfolioUser> PortfolioUser { get; set; }
        public virtual DbSet<MoneyCard> MoneyCards { get; set; }
        public virtual DbSet<UserMoneyCard> UsersMoneyCard { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockPrice> StockPrices { get; set; }
        public virtual DbSet<StockBuyAndSale> StockBuyAndSale { get; set; }

        public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

    }
}
