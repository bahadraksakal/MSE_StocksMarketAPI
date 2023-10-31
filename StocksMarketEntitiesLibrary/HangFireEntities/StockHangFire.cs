using Microsoft.EntityFrameworkCore;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketEntitiesLibrary.HangFireEntities
{
    [Index(nameof(User.Name), IsUnique = true)]
    public class StockHangFire
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
