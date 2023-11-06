namespace StocksMarketEntitiesLibrary.EmailEntities
{
    public class Emailer
    {
        public string smtpServer { get; set;}
        public int smtpPort { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public List<string> toUserEmails { get; set; }
        public string toUserEmail { get; set; }
        public bool enableSsl { get; set; }
    }
}
