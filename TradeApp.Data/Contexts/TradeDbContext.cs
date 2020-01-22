using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Models.TradeDbModels;

namespace TradeApp.Data.Contexts
{
    public class TradeDbContext : DbContext
    {
        public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Widget> Widget { get; set; }
        public DbSet<UserDashboard> UserDashboard { get; set; }
        public DbSet<UserDashboardWidget> UserDashboardWidget { get; set; }
        public DbSet<UserDashboardWidgetFilter> UserDashboardWidgetFilter { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("trd");

            ConfigureSoftDelete(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Modified:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ConfigureSoftDelete(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Widget>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserDashboard>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserDashboardWidget>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserDashboardWidgetFilter>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Filter>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Tag>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}