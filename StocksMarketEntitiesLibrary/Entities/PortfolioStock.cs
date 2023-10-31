namespace StockMarketEntitiesLibrary.Entities
{
    public class PortfolioStock
    {
        public int Id { get; set; }

        public int PortfolioId { get; set; }

        public int StockId { get; set; }

        public int Unit { get; set; }

        public virtual Portfolio Portfolio { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
