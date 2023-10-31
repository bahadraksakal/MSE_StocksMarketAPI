using Microsoft.EntityFrameworkCore;
using StocksMarketEntitiesLibrary.HangFireEntities;

namespace HangFireDbContextLibrary.Context
{
    public class HangFireDbContext : DbContext
    {
        public virtual DbSet<StockHangFire> StockHangFire { get; set; }

        public HangFireDbContext(DbContextOptions<HangFireDbContext> options) : base(options)
        {

        }
    }
}