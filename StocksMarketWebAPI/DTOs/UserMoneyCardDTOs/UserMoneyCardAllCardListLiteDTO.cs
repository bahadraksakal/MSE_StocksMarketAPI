namespace StocksMarketWebAPI.DTOs.UserMoneyCardDTOs
{
    public class UserMoneyCardAllCardListLiteDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int MoneyCardId { get; set; }

        public float MoneyCardBalance { get; set; }

        public DateTime MoneyCardCreationDate { get; set; }

        public DateTime? MoneyCardUsedDate { get; set; }

        public bool Status { get; set; }
    }
}
