namespace StocksMarketWebAPI.Entities
{
    public class MoneyCard
    {
        public int Id { get; set; }

        public float Balance { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UsedDate { get; set; }

        public virtual ICollection<UserMoneyCard> UserMoneyCard { get; set; }
    }
}
