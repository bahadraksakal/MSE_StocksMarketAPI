namespace StockMarketEntitiesLibrary.Entities
{
    public class StockPrice
    {
        public int Id { get; set; }

        public int StockId { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
