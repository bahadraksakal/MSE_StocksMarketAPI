namespace StocksMarketWebAPI.DTOs.PortfolioUserDTOs
{
    public class PortfolioUserDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? UserTel { get; set; }

        public string? UserRole { get; set; }

        public int PortfolioId { get; set; }

        public float? PortfolioBalance { get; set; }
    }
}
