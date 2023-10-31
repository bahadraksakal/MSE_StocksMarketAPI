namespace StockMarketEntitiesLibrary.Entities
{
    public class StockBuyAndSale
    {
        public int Id { get; set; }

        public int PortfolioId { get; set; }

        public int StockId { get; set; }

        public string Status { get; set; }

        public int Unit { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public virtual Stock Stock { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
