namespace StocksMarketWebAPI.DTOs.StockBuyAndSaleDTOs
{
    public class StockBuyAndSaleDTO
    {
        public int Id { get; set; }

        public int PortfolioId { get; set; }

        public float? PortfolioBalance { get; set; }

        public int StockId { get; set; }

        public string? StockName { get; set; }

        public int? StockUnit { get; set; }

        public bool? StockStatus { get; set; }

        public string Status { get; set; }

        public int Unit { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }
    }
}
