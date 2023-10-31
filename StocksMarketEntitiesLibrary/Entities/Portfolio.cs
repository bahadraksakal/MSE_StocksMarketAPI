namespace StockMarketEntitiesLibrary.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }

        public float Balance { get; set; }

        public virtual PortfolioUser PortfolioUsers { get; set; }
        public virtual ICollection<StockBuyAndSale> StockBuyAndSales { get; set; }
        public virtual ICollection<PortfolioStock> PortfoliosStocks{ get; set; }
    }
}
