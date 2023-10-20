namespace StocksMarketWebAPI.Entities
{
    public class Stock
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Unit { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<StockBuyAndSale> StockBuyAndSales { get; set; }
        public virtual ICollection<StockPrice> StockPrices { get; set; }
        public virtual ICollection<PortfolioStock> PortfolioStocks { get; set; }
    }
}
