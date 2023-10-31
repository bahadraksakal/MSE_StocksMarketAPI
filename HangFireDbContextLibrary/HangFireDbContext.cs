using Microsoft.EntityFrameworkCore;

namespace HangFireDbContextLibrary
{
    public class HangFireDbContext : DbContext
    {
        public HangFireDbContext(DbContextOptions<HangFireDbContext> options) : base(options)
        {

        }
    }
}