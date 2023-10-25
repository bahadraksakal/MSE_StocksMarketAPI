namespace StocksMarketWebAPI.DTOs.StockPriceDTOs
{
    public class StockPriceDTO
    {
        public int Id { get; set; }

        public int StockId { get; set; }

        public string StockName { get; set; }

        public int StockUnit { get; set; }

        public bool StockStatus { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }
    }
}
