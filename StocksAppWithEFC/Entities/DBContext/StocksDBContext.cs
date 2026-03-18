using Microsoft.EntityFrameworkCore;
using Entities;

namespace Entities.DBContext
{
    public class StocksDBContext:DbContext
    {
        //sending options to parent constructor
        public StocksDBContext(DbContextOptions<StocksDBContext> options):base(options) { }

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<BuyOrder>().HasKey("BuyOrderID");

            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
            modelBuilder.Entity<SellOrder>().HasKey("SellOrderID");
        }
    }
}
