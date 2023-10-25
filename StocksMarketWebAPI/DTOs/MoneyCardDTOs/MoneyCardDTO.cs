namespace StocksMarketWebAPI.DTOs.MoneyCardDTOs
{
    public class MoneyCardDTO
    {
        public int Id { get; set; }

        public float Balance { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UsedDate { get; set; }
    }
}
