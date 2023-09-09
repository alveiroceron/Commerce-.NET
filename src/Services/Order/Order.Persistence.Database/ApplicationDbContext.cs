using Microsoft.EntityFrameworkCore;
using Order.Persistence.Database.Configuration;

namespace Order.Persistence.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Database schema
            builder.HasDefaultSchema("Order");

            ModelConfig(builder);
        }

        public DbSet<Domain.Order> Orders { get; set; }
        public DbSet<Domain.OrderDetail> OrderDetail { get; set; }
        private static void ModelConfig(ModelBuilder modelBuilder)
        {
            new OrderConfiguration(modelBuilder.Entity<Domain.Order>());
            new OrderDetailConfiguration(modelBuilder.Entity<Domain.OrderDetail>());
        }
    }
}
