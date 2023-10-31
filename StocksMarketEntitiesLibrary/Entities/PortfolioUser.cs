namespace StockMarketEntitiesLibrary.Entities
{
    public class PortfolioUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PortfolioId { get; set; }

        public virtual Portfolio Portfolio { get; set; }

        public virtual User User { get; set; }
    }
}
