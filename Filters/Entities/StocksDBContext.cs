using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StocksDBContext:DbContext
    {
        public StocksDBContext(DbContextOptions<StocksDBContext> options):base(options)
        {

        }

        public virtual DbSet<BuyOrder> BuyOrders { get; set; }
        public virtual DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Setting tables
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");

            modelBuilder.Entity<BuyOrder>().HasKey("BuyOrderID");
            modelBuilder.Entity<SellOrder>().HasKey("SellOrderID");
        }
    
    }
}
