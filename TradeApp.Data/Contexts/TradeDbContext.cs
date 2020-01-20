using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Models;
using TradeApp.Data.Models.TradeDbModels;

namespace TradeApp.Data.Contexts
{
    public class TradeDbContext : DbContext
    {
        public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }

        public DbSet<User> User { get; set; }
        public DbSet<Widget> Widget { get; set; }
        public DbSet<UserDashboard> UserDashboard { get; set; }
        public DbSet<UserDashboardWidget> UserDashboardWidget { get; set; }
        public DbSet<UserDashboardWidgetFilter> UserDashboardWidgetFilter { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
