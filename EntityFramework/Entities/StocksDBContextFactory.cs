using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Entities
{
    public class StocksDBContextFactory : IDesignTimeDbContextFactory<StocksDBContext>
    {
        public StocksDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StocksDBContext>();

            // Use the same connection string as in appsettings.json
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StockDatabase;Integrated Security=True;TrustServerCertificate=True;");

            return new StocksDBContext(optionsBuilder.Options);
        }
    }
}
