using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StocksAppEntityFrameWork;

namespace Entities
{
    public class StocksDBContext:DbContext
    {
        public StocksDBContext(DbContextOptions<StocksDBContext> options): base(options) 
        { }

        public DbSet<BuyOrder> BuyOrders { get; set;}
        public DbSet<SellOrder> SellOrders { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");

            modelBuilder.Entity<BuyOrder>().HasKey("BuyOrderID");
            modelBuilder.Entity<SellOrder>().HasKey("SellOrderID");
        }
    }
}
