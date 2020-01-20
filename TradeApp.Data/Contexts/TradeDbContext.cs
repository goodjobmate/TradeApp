using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Models;

namespace TradeApp.Data.Contexts
{
    public class TradeDbContext : DbContext
    {
        public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options)
        {

        }

        public DbSet<Tag> Tags { get; set; }
    }
}
