namespace StocksMarketWebAPI.Entities
{
    public class UserMoneyCard
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MoneyCardId { get; set; }

        public bool Status { get; set; }

        public virtual MoneyCard MoneyCard { get; set; }

        public virtual User User { get; set; }
    }
}
