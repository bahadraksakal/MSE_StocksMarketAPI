namespace StocksMarketWebAPI.DTOs.PortfolioStockDTOs
{
    public class PortfolioStockDTO
    {
        public int Id { get; set; }

        public int PortfolioId { get; set; }

        public float? PortfolioBalance { get; set; }

        public int StockId { get; set; }

        public string? StockName { get; set; }

        public int? StockUnit { get; set; }

        public bool? StockStatus { get; set; }

        public int Unit { get; set; }

        public bool? IsTracked { get; set; }
    }
}
