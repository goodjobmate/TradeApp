using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.Data.Contexts
{
    public class BaseMetaDbContext : DbContext
    {
        public BaseMetaDbContext(DbContextOptions<BaseMetaDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Regulation> Regulations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CrossReference> CrossReferences { get; set; }
        public DbSet<GroupCrossReference> GroupCrossReferences { get; set; }
        //TODO : will be removed.
        public DbQuery<ServerGroup> ServerGroups { get; set; }
    }
}
