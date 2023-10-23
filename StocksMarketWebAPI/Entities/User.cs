using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StocksMarketWebAPI.Entities
{
    [Index(nameof(User.Name), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        public string Role { get; set; }

        public virtual PortfolioUser PortfolioUser { get; set; }
        public virtual ICollection<UserMoneyCard> UserMoneyCards { get; set; }
    }
}
