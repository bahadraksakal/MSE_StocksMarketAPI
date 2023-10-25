namespace StocksMarketWebAPI.DTOs.UserMoneyCardDTOs
{
    public class UserMoneyCardDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? UserTel { get; set; }

        public string? UserRole { get; set; }

        public int MoneyCardId { get; set; }

        public float? MoneyCardBalance { get; set; }

        public DateTime? MoneyCardCreationDate { get; set; }

        public DateTime? MoneyCardUsedDate { get; set; }

        public bool Status { get; set; }
    }
}
